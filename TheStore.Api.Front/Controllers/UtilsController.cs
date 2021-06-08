// a.snegovoy@gmail.com

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Workers;

namespace TheStore.Api.Front.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public sealed class UtilsController : ControllerBase
    {
        [ HttpGet ]
        public void StartSitemapGeneration()
        {
            UtilsWorker.StartSitemapGeneration();
        }
    }
}