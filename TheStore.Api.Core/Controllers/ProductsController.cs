// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using Common.Settings;

using Microsoft.AspNetCore.Mvc;

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
            var worker = new ProductWorker( id, _settings.ElasticSearchClientSettings );
            return worker.Get();
        }

        [ HttpGet ]
        [ Route( "RemoveFromCategory" ) ]
        public IActionResult RemoveFromCategory(
            string id,
            string categoryId )
        {
            var worker = new ProductWorker( id, _settings.ElasticSearchClientSettings );
            return worker.RemoveFromCategory( categoryId );
        }
        
    }
}