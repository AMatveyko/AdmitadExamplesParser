// a.snegovoy@gmail.com

using Microsoft.AspNetCore.Mvc;

namespace TheStoreApi.Controllers
{
    public class Test : Controller
    {
        // GET
        public IActionResult Index()
        {
            return new ContentResult();
        }
    }
}