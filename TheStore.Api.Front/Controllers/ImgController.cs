// a.snegovoy@gmail.com

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Workers;

namespace TheStore.Api.Front.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class ImgController : ControllerBase
    {

        private readonly ImageWorker _worker;

        public ImgController( ImageWorker worker ) => _worker = worker;

        [ HttpGet ]
        [ Route( "GetByUrl" ) ]
        public async Task<IActionResult> GetByUrl( string url ) =>
            await _worker.GetByUrl( url );

        [ HttpGet ]
        [ Route( "GetById" ) ]
        public async Task<IActionResult> GetById(
            string productId ) =>
            await _worker.GetById( productId );

    }
}