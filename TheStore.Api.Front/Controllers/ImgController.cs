// a.snegovoy@gmail.com

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Entity;
using TheStore.Api.Front.Workers;

namespace TheStore.Api.Front.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class ImgController : ControllerBase
    {

        private readonly Proxies _proxies;

        public ImgController( Proxies proxies ) => _proxies = proxies;

        [ HttpGet ]
        public async Task<IActionResult> Get( string url )
        {
            return await ImageWorker.Get( url, _proxies );
        }

    }
}