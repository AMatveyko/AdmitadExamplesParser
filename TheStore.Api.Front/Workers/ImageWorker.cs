// a.snegovoy@gmail.com

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using Common.Elastic.Workers;
using Common.Extensions;
using Common.Helpers;

using Microsoft.AspNetCore.Mvc;

using NLog;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using TheStore.Api.Front.Entity;

using Web.Common.Entities;
using Web.Common.Helpers;
using Web.Common.Workers;

namespace TheStore.Api.Front.Workers
{

    public sealed class ImageWorker
    {

        private static readonly Logger Logger = LogManager.GetLogger( "DownloadError" );
        private static readonly Logger RequestLogger = LogManager.GetLogger( "Request" );
        private static readonly Logger Statistics = LogManager.GetLogger( "ProxyStatistics" );
        private static readonly Regex MD5Pattern = new Regex( "^[0-9a-fA-F]{32}$", RegexOptions.Compiled ); 

        private const string PhotoDelimiter = "_";
        private const string CacheDirectoryTemplate = "cache/{0}/{1}/{2}.jpg";

        private readonly Proxies _proxies;
        private readonly IIndexClient _indexClient;

        public ImageWorker( Proxies proxies, IIndexClient indexClient ) =>
            ( _proxies, _indexClient ) = ( proxies, indexClient );


        public async Task<IActionResult> GetById( string productId )
        {
            return await TryExecute( DoGetById, productId );
        }

        public async Task<IActionResult> GetByUrl( string rawUrl )
        {
            var url = HttpUtility.UrlDecode( rawUrl );
            return await TryExecute( DoGetByUrl, url );
        }
        
        private async Task<IActionResult> DoGetById( string productId )
        {
            
            RequestLogger.Info( productId );
            
            productId = productId.Replace( ".jpg", string.Empty );
            
            var ( id, number ) = GetPhotoInfo( productId );

            if( MD5Pattern.IsMatch( id ) == false ) {
                return GetNotFoundResult();
            }

            // пробуем поискать в старом кеше
            if( number == 1 ) {
                var oldPath = GetPathByHash( id );
                if( TryGetFromFs( oldPath, out var imageFromOldCache ) ) {
                    return GetResult( imageFromOldCache );
                }
            }

            var path = GetPathByPhotoId( $"{id}{PhotoDelimiter}{number}" );
            if( TryGetFromFs( path, out var imageFromCache ) ) {
                return GetResult( imageFromCache );
            }
            
            var productsPhotoNewIndex = await _indexClient.GetProductsPhotoAsync( id );
            var productsPhotoOldIndex = await _indexClient.GetProductsPhotoAsync( id, "products-old-1" );
            var productsPhoto = productsPhotoNewIndex ?? productsPhotoOldIndex;

            if( productsPhoto == null ) {
                return GetNotFoundResult();
            }
            
            if( productsPhoto.Photos.Count < number ) {
                return GetNotFoundResult();
            }

            var image = await GetImage( productsPhoto.Photos[ number - 1 ], path );
            return GetResult( image );

        }
        
        private async Task<IActionResult> DoGetByUrl( string url )
        {
            
            RequestLogger.Info( url );
            
            var path = GetPathByUrl( url );
            if( TryGetFromFs( path, out var imageFromFs ) ) {
                return GetResult( imageFromFs );
            }

            var image = await GetImage( url, path );
            return GetResult( image );
        }

        private async Task<byte[]> GetImage( string url, string cachePath )
        {
            var imageInfo = await DownloadImage( url, _proxies );
            
            if( imageInfo.IsNotFound == false ) {
                SaveImage( cachePath, imageInfo.Image );
            }
            
            return imageInfo.Image;
        }
        
        #region FS

        private static void SaveImage( string path, byte[] image ) {
            if( Directory.Exists( Path.GetDirectoryName( path ) ) == false ) {
                Directory.CreateDirectory( Path.GetDirectoryName( path ) );
            }
            File.WriteAllBytes( path, image );
        }

        private static bool TryGetFromFs( string path, out byte[] file ) {
            if( File.Exists( path ) ) {
                // чтобы понимать какие файлы довно не использовались
                File.SetCreationTimeUtc( path, DateTime.UtcNow );
                file = File.ReadAllBytes( path );
                return true;
            }

            file = new byte[0];
            return false;
        }
        
        #endregion

        #region Download

        private static async Task<ImageInfo> DownloadImage( string url, Proxies proxyInfos )
        {
            var attempt = 0;
            var result = new byte[0];
            var proxies = ProxyDistributor.GetProxies( proxyInfos.Infos );
            foreach( var proxy in proxies ) {
                attempt++;
                try {
                    result = await DoDownloadImage( url, proxy );
                    Statistics.Info( $"{proxy?.Url ?? "self"} {url} Ok" );
                    return new ImageInfo( result );
                }
                catch( Exception e ) {
                    if( e.Message.Contains( "(Not Found)" ) ) {
                        Statistics.Info( $"{proxy?.Url ?? "self"} {url} NotFound" );
                        return new ImageInfo( GetNotFound(), true );
                    }
                    Statistics.Info( $"{proxy?.Url ?? "self"} {url} Error" );
                    Logger.Error( e, $"{url} {attempt} attempt" );
                }
            }
            
            return new ImageInfo( GetNotFound(), true );
        }

        private static byte[] GetNotFound()
        {
            const string path = "i/nophoto.jpg";
            return File.Exists( path ) ? File.ReadAllBytes( path ) : new byte[0];
        }

        private static async Task<byte[]> DoDownloadImage( string url, ProxyInfo proxyInfo )
        {
            var imageBytes = await WebRequester.Request( url, proxyInfo );
            return ResizeImageIfNeed( imageBytes );
        }
        
        private static byte[] ResizeImageIfNeed( byte[] imageByte ) {
            var image = Image.Load( imageByte );
            // var resizeOptions = new ResizeOptions {
            //     Mode = ResizeMode.Crop,
            //     Size = new Size( 322, 450 ),
            // };
            // image.Mutate( a => a.Resize( resizeOptions ) );

            var finalImage = IsNeedResize( image ) ? DoResizeImage( image ) : image;
            
            using var ms = new MemoryStream();
            finalImage.Save( ms, new JpegEncoder() );
            return ms.ToArray();
        }

        private static Image<Rgba32> DoResizeImage( Image<Rgba32> image ) {
            var resizeOptions = new ResizeOptions {
                // Mode = ResizeMode.Crop,
                Mode = ResizeMode.Max,
                Size = new Size( 800, 600 ),
            };
            image.Mutate( a => a.Resize( resizeOptions ) );
            return image;
        }
        
        private static bool IsNeedResize( Image<Rgba32> image ) => image.Width > 800 || image.Height > 600;
        
        #endregion

        #region Routine

        private static IActionResult GetResult( byte[] image ) =>
            new FileContentResult( image, "image/jpeg" );

        private static string GetPathByUrl( string url )
        {
            var hash = GetHash( url );
            return GetPathByHash( hash );
        }

        private static string GetPathByPhotoId( string photoId )
        {
            var hash = GetHash( photoId );
            return GetPathByHash( hash );
        }
        
        private static string GetPathByHash( string hash )
        {
            var firstDirectory = hash[ ..2 ];
            var secondDirectory = hash[ 2..4 ];
            return string.Format( CacheDirectoryTemplate, firstDirectory, secondDirectory, hash );
        }

        private static string GetHash( string data ) => HashHelper.GetMd5Hash( data ).ToLower();

        private static IActionResult GetNotFoundResult() => GetResult( GetNotFound() );
        
        private static async Task<IActionResult> TryExecute( Func<string,Task<IActionResult>> func, string data ) {
            if( data.IsNullOrWhiteSpace() ) {
                return GetNotFoundResult();
            }
            try {
                return await func( data );
            }
            catch( Exception e ) {
                Logger.Error( e, data );
                throw;
            }
        }

        private static ( string, int ) GetPhotoInfo( string data )
        {
            var parts = data.Split( PhotoDelimiter );
            if( parts.Length < 2 ) {
                return ( data, 1 );
            }

            return int.TryParse( parts[ 1 ], out var pictureNumber )
                ? ( parts[ 0 ], pictureNumber )
                : ( data, 1 );
        }
        
        #endregion

    }
}