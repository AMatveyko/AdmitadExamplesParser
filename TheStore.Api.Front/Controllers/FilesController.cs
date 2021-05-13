// a.snegovoy@gmail.com

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Workers;

namespace TheStore.Api.Front.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class FilesController : ControllerBase
    {
        [ HttpGet ]
        [ Route( "GetPageStatistics" ) ]
        public IActionResult GetPageStatistics()
        {
            return FileWorker.GetPageStatistics();
        }
        
    }
}