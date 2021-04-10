// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Admitad.Converters;

using AdmitadCommon.Entities;

using ElasticSearchData;

using FatApi.Model;

namespace FatApi.Workers
{
    public sealed class ProductFromOffersFactory : IProductFactory
    {
        private readonly IOfferRepository _repository;

        public ProductFromOffersFactory(
            IOfferRepository repository )
        {
            _repository = repository;
        }

        public Product Get(
            string id )
        {
            var offers = _repository.GetByProductId( id );
            return ProductConverter.CollectProduct( offers );
        }

        public ProductSearchResult ProductSearch(
            SearchParameters parameters,
            int offSet,
            int frameSize )
        {
            var result = new ProductSearchResult();
            var offers = _repository.OffersSearch( parameters );
            var offersDictionary = CreateProductsDictionary( offers );
            var frame = offersDictionary.Keys.Skip( offSet ).Take( frameSize );
            result.Products = frame.Select( key => ProductConverter.CollectProduct( offersDictionary[ key ] ) )
                .ToList();
            result.FrameSize = frameSize;
            result.OffSet = offSet;
            result.TotalCount = offersDictionary.Keys.Count;
            return result;
        }

        private Dictionary<string, List<Offer>> CreateProductsDictionary(
            List<Offer> offers )
        {
            var dictionary = new Dictionary<string, List<Offer>>();
            foreach( var offer in offers ) {
                if( dictionary.ContainsKey( offer.ProductId ) == false )
                    dictionary[ offer.ProductId ] = new List<Offer>();
                dictionary[ offer.ProductId ].Add( offer );
            }

            return dictionary;
        }
    }
}