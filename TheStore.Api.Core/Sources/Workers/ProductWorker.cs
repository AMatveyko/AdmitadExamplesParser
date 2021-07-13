// a.snegovoy@gmail.com

using System;

using Admitad.Converters.Workers;

using AdmitadCommon.Entities;

using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;
using Common.Settings;

using Microsoft.AspNetCore.Mvc;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class ProductWorker
    {

        private ElasticSearchClientSettings _settings;
        private string _productId;
        
        public ProductWorker( string productId, ElasticSearchClientSettings settings )
        {
            _productId = productId;
            _settings = settings;
        }
        
        public IActionResult Get()
        {
            var client = new ElasticSearchClient<IIndexedEntities>( _settings, new BackgroundBaseContext( _productId, "get:product") );
            var product = client.GetProduct( _productId );
            return new JsonResult( product );
        }

        public IActionResult RemoveFromCategory( string categoryId )
        {
            try {
                var client = IndexClient.CreateProductWorker(
                    _settings,
                    new BackgroundBaseContext( _productId, "removeFromCategory:product" ) );
                client.RemoveCategory( _productId, categoryId );
                return new OkResult();
            }
            catch( Exception e ) {
                return new ContentResult {
                    Content = e.Message
                };
            }
        }
    }
}