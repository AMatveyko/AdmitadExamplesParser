// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using Common.Settings;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using TheStore.Api.Core.Sources.Workers;

namespace TheStore.Api.Core.Controllers
{
    [ ApiController ]
    [ Route( "[controller]" ) ]
    public class ProductsController : ControllerBase
    {

        private readonly ProcessorSettings _settings;
        
        public ProductsController( ProcessorSettings settings )
        {
            _settings = settings;
        }

        [ HttpGet ]
        [ Route( "Get" ) ]
        public IActionResult Get( string id )
        {
            var worker = GetWorker( id );
            return worker.Get();
        }

        [ HttpGet ]
        [ Route( "RemoveFromCategory" ) ]
        public IActionResult RemoveFromCategory(
            string id,
            string categoryId )
        {
            var worker = GetWorker( id );
            return worker.RemoveFromCategory( categoryId );
        }

        [ HttpGet ]
        [ Route( "RemoveFromTag" ) ]
        public IActionResult RemoveFromTag( string id, string tagId )
        {
            var worker = GetWorker( id );
            return worker.RemoveFromTag( tagId );
        }
        
        private ProductWorker GetWorker(string id) {
            var productId = Sources.Helpers.UrlHelper.GetProductIdFromUrl( id );
            return new ProductWorker( productId, _settings.ElasticSearchClientSettings );
        }
        
    }
}