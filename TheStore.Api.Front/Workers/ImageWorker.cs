// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNetCore.Mvc;

using NLog;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

using TheStore.Api.Front.Entity;

namespace TheStore.Api.Front.Workers
{

    public static class ImageWorker
    {

        private static readonly ProxyInfo Proxy = new ProxyInfo( "http://193.47.45.60:8000", "U0acrR", "qHYQ0o" );
        
        private static readonly Logger Logger = LogManager.GetLogger( "DownloadError" );

        private const string CacheDirectoryTemplate = "cache/{0}/{1}/{2}.jpg";
        private static readonly Dictionary<string, string> HashCache = new Dictionary<string, string>();
        
        public static async Task<IActionResult> Get( string rawUrl )
        {
            var url = HttpUtility.UrlDecode( rawUrl );
            try {
                if( TryGetFromFs( url, out var imageFromFs ) ) {
                    return GetResult( imageFromFs );
                }

                var image = await DownloadImage( url );
                SaveImage( url, image );
                return GetResult( image );
            }
            catch( Exception e ) {
                Logger.Error( e, url );
                throw new Exception( "File error!" );
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

        private static async Task<byte[]> DownloadImage( string url )
        {
            try {
                return await DoDownloadImage( url, false );
            }
            catch( Exception e ) {
                Logger.Error( e, $"{url} first attempt" );
                return await DoDownloadImage( url, true );
            }
        }
        
        private static async Task<byte[]> DoDownloadImage( string url, bool useProxy )
        {
            var uri = new Uri( url );
            if( useProxy ) {
                var proxy = new WebProxy
                {
                    Address = new Uri( "http://193.47.45.60:8000" ),
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,
                    
                    Credentials = new NetworkCredential(
                        userName: "U0acrR",
                        password: "qHYQ0o" )
                };
                
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = proxy,
                };
                
                using var httpClient = new HttpClient( httpClientHandler );
                var imageBytes = await httpClient.GetByteArrayAsync( uri );
                return ResizeImage( imageBytes );
            }
            else {
                using var httpClient = new HttpClient();
                var imageBytes = await httpClient.GetByteArrayAsync( uri );
                return ResizeImage( imageBytes );
            }
            //return new FileContentResult( resizedImage, "image/jpeg" );
        }
        
        private static byte[] ResizeImage( byte[] imageByte )
        {
            var image = Image.Load( imageByte );
            image.Mutate( x => x.Resize( 322, 450 ) );
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
            if( HashCache.ContainsKey( url ) ) {
                return HashCache[ url ];
            }

            HashCache[ url ] = CreateMD5( url );
            return HashCache[ url ];
        }
        
        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
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