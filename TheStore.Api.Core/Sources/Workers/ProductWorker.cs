// a.snegovoy@gmail.com

using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using Common.Settings;

using Microsoft.AspNetCore.Mvc;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ProductWorker
    {

        private ElasticSearchClientSettings _settings;
        
        public ProductWorker( ElasticSearchClientSettings settings )
        {
            _settings = settings;
        }
        
        public IActionResult Get( string id )
        {
            var client = new ElasticSearchClient<IIndexedEntities>( _settings, new BackgroundBaseContext( id, "get:product") );
            var product = client.GetProduct( id );
            return new JsonResult( product );
        }
    }
}