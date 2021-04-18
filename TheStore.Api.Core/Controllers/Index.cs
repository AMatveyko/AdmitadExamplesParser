// a.snegovoy@gmail.com

using Microsoft.AspNetCore.Mvc;

namespace TheStore.Api.Core.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class Index : ControllerBase
    {
        [ HttpGet ]
        public static IActionResult Check( string test )
        {
            return new ContentResult {
                Content = "Ok"
            };
        }
    }
}