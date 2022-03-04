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

        public IActionResult RemoveFromTag( string tagId ) {
            var client = CreateProductWorker( "removeFromTag:product" );
            return Execute( () => client.RemoveTag( _productId, tagId ) );
        }
        
        public IActionResult RemoveFromCategory( string categoryId ) {
            var client = CreateProductWorker( "removeFromCategory:product" );
            return Execute( () => client.RemoveCategory( _productId, categoryId ) );
        }

        private IActionResult Execute( Action action ) {
            try {
                action();
                return new OkResult();
            }
            catch( Exception e ) {
                return new ContentResult {
                    Content = e.Message
                };
            }
        }
        
        private IIndexProductWorker CreateProductWorker( string contextName ) =>
            IndexClient.CreateProductWorker( _settings, new BackgroundBaseContext( _productId, contextName ) ); 
    }
}