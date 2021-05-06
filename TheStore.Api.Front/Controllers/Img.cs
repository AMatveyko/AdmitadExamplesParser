// a.snegovoy@gmail.com

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Workers;

namespace TheStoreApi.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class Img : ControllerBase
    {

        [ HttpGet ]
        public async Task<IActionResult> Get( string url )
        {
            return await ImageWorker.Get( url );
        }

    }
}