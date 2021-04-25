// a.snegovoy@gmail.com

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace TheStoreApi.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class ImageWorker : ControllerBase
    {

        [ HttpGet ]
        public async Task<IActionResult> DownloadByUrl( string url )
        {
            try {
                var uri = new Uri( url );
                using var httpClient = new HttpClient();
                var imageBytes = await httpClient.GetByteArrayAsync( uri );
                //var resizedImage = ResizeImage( imageBytes );
                return File( imageBytes, "image/jpeg" );
            }
            catch( Exception e ) {
                return new ContentResult { Content = e.Message };
            }
        }

        private Byte[] ResizeImage( Byte[] imageByte )
        {
            var image = Image.Load( imageByte );
            image.Mutate( x => x.Resize( 200, 200 ) );
            using var ms = new MemoryStream();
            image.Save( ms, new JpegEncoder() );
            return ms.ToArray();
        }
        
    }
}