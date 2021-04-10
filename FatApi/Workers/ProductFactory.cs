// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities;

using ElasticSearchData;

using FatApi.Model;

namespace FatApi.Workers
{
    public class ProductFactory : IProductFactory
    {
        private readonly IProductRepository _repository;

        public ProductFactory(
            IProductRepository repository )
        {
            _repository = repository;
        }

        public Product Get(
            string id )
        {
            return _repository.GetProduct( id );
        }

        public ProductSearchResult ProductSearch(
            SearchParameters parameters,
            int offSet,
            int frameSize )
        {
            throw new NotImplementedException();
        }
    }
}