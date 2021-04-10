// a.snegovoy@gmail.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;

using Nest;

namespace ElasticSearchData
{
    public sealed class OfferRepository : IOfferRepository
    {

        private readonly ElasticClient _client;
        
        public OfferRepository( string url ) {
            var clientSettings = new ConnectionSettings( new Uri( url ) )
                .DefaultIndex( "offers" );
            _client = new ElasticClient( clientSettings );
        }

        public List<Offer> GetByProductId( string id )
        {
            var searchResponse = _client.Search<Offer>(
                s => 
                    s.Query( 
                        q => 
                            q.Match( 
                                m => 
                                    m.Field( SearchParameters.ProductIdField ).Query( id ) ) ).Size( 10000 ) );
            return searchResponse.Documents.ToList();
        }

        public List<Offer> OffersSearch(
            SearchParameters parameters )
        {
            var searchResponse = _client.Search<Offer>(
                s => s.Size(100).Query(
                    q => q.DisMax(
                        dm => dm.Queries( GetMatches( parameters ) ) ) ) );
            return searchResponse.Documents.ToList();
        }

        private static Func<QueryContainerDescriptor<Offer>, QueryContainer> GetMatch(
            string field,
            string text ) => dq => dq.Match( m => m.Field( field ).Query( text ) );

        private IEnumerable<Func<QueryContainerDescriptor<Offer>,QueryContainer>> GetMatches(
            SearchParameters parameters ) {
            if( parameters.Descriptions.IsNotNullOrWhiteSpace() ) {
                yield return GetMatch( SearchParameters.DescriptionField, parameters.Descriptions );
            }

            if( parameters.ShopId > 0 ) {
                yield return GetMatch( SearchParameters.ShopIdField, parameters.ShopId.ToString() );
            }

            if( parameters.VendorName.IsNotNullOrWhiteSpace() ) {
                yield return GetMatch( SearchParameters.VendorNameField, parameters.VendorName );
            }
        }
    }
}