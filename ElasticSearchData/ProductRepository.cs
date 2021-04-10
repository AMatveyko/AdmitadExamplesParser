// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

using AdmitadCommon.Entities;

using Nest;

namespace ElasticSearchData
{
    public class ProductRepository : IProductRepository
    {

        private readonly ElasticClient _client;
        
        public ProductRepository( string url )
        {
            var clientSettings = new ConnectionSettings( new Uri( url ) ).DefaultIndex( Product.IndexName );
            _client = new ElasticClient( clientSettings );
        }
        
        public Product GetProduct( string id )
        {
            return _client.Get<Product>( id ).Source;
        }

        public List<Product> ProductSearch( SearchParameters parameters, int offSet, int size )
        {
            throw new System.NotImplementedException();
        }
    }
}