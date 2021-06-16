// a.snegovoy@gmail.com

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNetCore.Mvc;

using NLog;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

using TheStore.Api.Front.Entity;

using Web.Common.Entities;
using Web.Common.Helpers;
using Web.Common.Workers;

namespace TheStore.Api.Front.Workers
{

    internal static class ImageWorker
    {

        private static readonly Logger Logger = LogManager.GetLogger( "DownloadError" );
        private static readonly Logger Statistics = LogManager.GetLogger( "ProxyStatistics" );

        private const string CacheDirectoryTemplate = "cache/{0}/{1}/{2}.jpg";

        public static async Task<IActionResult> Get( string rawUrl, Proxies proxies )
        {
            var url = HttpUtility.UrlDecode( rawUrl );
            try {
                if( TryGetFromFs( url, out var imageFromFs ) ) {
                    return GetResult( imageFromFs );
                }

                var image = await DownloadAndSaveImage( url, proxies );
                //Task.Run( () => SaveImage( url, image ) );
                return GetResult( image );
            }
            catch( Exception e ) {
                Logger.Error( e, url );
                throw;
            }
        }

        #region FS

        private static void SaveImage( string url, byte[] image ) {
            var path = GetPath( url );
            if( Directory.Exists( Path.GetDirectoryName( path ) ) == false ) {
                Directory.CreateDirectory( Path.GetDirectoryName( path ) );
            }
            File.WriteAllBytes( path, image );
        }
        
        private static bool TryGetFromFs( string url, out byte[] file )
        {
            var path = GetPath( url );
            if( File.Exists( path ) ) {
                file = File.ReadAllBytes( path );
                return true;
            }

            file = new byte[0];
            return false;
        }
        
        
        #endregion

        #region Download

        private static async Task<byte[]> DownloadAndSaveImage( string url, Proxies proxyInfos )
        {
            var attempt = 0;
            var result = new byte[0];
            var proxies = ProxyDistributor.GetProxies( proxyInfos.Infos );
            foreach( var proxy in proxies ) {
                attempt++;
                try {
                    result = await DoDownloadImage( url, proxy );
                    Statistics.Info( $"{proxy?.Url ?? "self"} {url} Ok" );
                    SaveImage( url, result );
                    return result;
                }
                catch( Exception e ) {
                    if( e.Message.Contains( "(Not Found)" ) ) {
                        Statistics.Info( $"{proxy?.Url ?? "self"} {url} NotFound" );
                        return GetNotFound();
                    }
                    Statistics.Info( $"{proxy?.Url ?? "self"} {url} Error" );
                    Logger.Error( e, $"{url} {attempt} attempt" );
                    // if( e is UnknownImageFormatException ) {
                    //     File.WriteAllBytes( $"logs/data/{url.Split('/').ToList().LastOrDefault() ?? "null"}", result );
                    // }
                }
            }
            
            return GetNotFound();
        }

        private static byte[] GetNotFound()
        {
            const string path = "i/nophoto.jpg";
            return File.Exists( path ) ? File.ReadAllBytes( path ) : new byte[0];
        }
        
        private static async Task<byte[]> DoDownloadImage( string url, ProxyInfo proxyInfo )
        {
            var imageBytes = await WebRequester.Request( url, proxyInfo );
            return ResizeImage( imageBytes );
        }
        
        private static byte[] ResizeImage( byte[] imageByte )
        {
            var image = Image.Load( imageByte );
            var resizeOptions = new ResizeOptions {
                Mode = ResizeMode.Crop,
                Size = new Size( 322, 450 ),
            };
            image.Mutate( a => a.Resize( resizeOptions ) );
            using var ms = new MemoryStream();
            image.Save( ms, new JpegEncoder() );
            return ms.ToArray();
        }
        
        #endregion

        #region Routine

        private static IActionResult GetResult( byte[] image ) => new FileContentResult( image, "image/jpeg" ); 
        
        private static string GetPath( string url )
        {
            var hash = GetHash( url ).ToLower();
            var firstDirectory = hash[ ..2 ];
            var secondDirectory = hash[ 2..4 ];
            return string.Format( CacheDirectoryTemplate, firstDirectory, secondDirectory, hash );
        }
        
        private static string GetHash( string url )
        {
            return CreateMD5( url );
        }
        
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                foreach( var t in hashBytes ) {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }
        }
        
        #endregion

    }
}