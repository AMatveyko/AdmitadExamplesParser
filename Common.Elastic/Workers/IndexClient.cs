// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Common.Api;
using Common.Entities;
using Common.Settings;

using Nest;

namespace Common.Elastic.Workers
{
    public sealed class IndexClient : IIndexClient
    {

        #region Data

        private readonly ElasticClient _client;
        private readonly BackgroundBaseContext _context;
        private readonly ElasticSearchClientSettings _settings;

        #endregion




        #region Ctors

        private IndexClient(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context )
        {
            var clientSettings =
                new ConnectionSettings( new Uri( settings.ElasticSearchUrl ) ).DefaultIndex( settings.DefaultIndex );
            _client = new ElasticClient( clientSettings );
            _context = context;
            _settings = settings;
            Setup();
        }

        #endregion




        #region Initialization

        private void Setup()
        {
            _client.Cluster.PutSettings(
                descriptor => descriptor.Transient( f => f.Add( "script.max_compilations_rate", "10000/1m" ) ) );
        }

        #endregion




        public static IIndexClient Create(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context ) =>
            new IndexClient( settings, context );

        public List<ProductPart> SearchProductsByOffersIds(
            string[] ids )
        {
            const string time = "1m";

            var preparedIds = ids.Select( i => i.ToLower() );

            var searchResponse = _client.Search<ProductPart>(
                s => s.Size( 1000 ).Query(
                        q => q.Bool(
                            b => b.Must(
                                t => t.Terms( terms => terms.Field( p => p.OfferIds ).Terms( preparedIds ) ) ) ) )
                    .Scroll( time ) );
            CheckResponse( searchResponse );

            var products = new List<ProductPart>();

            while( searchResponse.Documents.Any() ) {
                products.AddRange( searchResponse.Documents );
                searchResponse = _client.Scroll<ProductPart>( time, searchResponse.ScrollId );
                CheckResponse( searchResponse );
            }

            return products;
        }

        public void UpdateProductsAfterDeletingOffers< T >(
            IReadOnlyList<T> products )
            where T : ProductPart
        {
            var result = _client.Bulk(
                b => b.UpdateMany<ProductPart>(
                    products,
                    (
                        descriptor,
                        part ) => descriptor.Doc( part ) ) );
            CheckResponse( result );
        }

        public List<ProductPart> GetProductsByIds(
            string[] ids )
        {
            var result = _client.MultiGet( d => d.Index( _settings.DefaultIndex ).GetMany<ProductPart>( ids ) );
            CheckResponse( result );
            return result.Hits.Where( h => h.Found ).Select( h => h.Source as ProductPart ).ToList();
        }

        public void UpsertProducts< T >(
            IReadOnlyList<T> products )
            where T : class, IIndexedEntities
        {
            var position = 0;
            while( position < products.Count ) {
                var portion = products.Skip( position ).Take( _settings.FrameSize );
                var result = _client.Bulk(
                    b => b.UpdateMany<T, IProductForIndex>(
                        portion,
                        (
                            descriptor,
                            entity ) => descriptor.Upsert( entity ).DocAsUpsert().Doc( ( IProductForIndex ) entity )
                            .Routing( entity.RoutingId ) ) );
                position += _settings.FrameSize;
                if( result.IsValid == false ) {
                    _context.AddMessage( $"Upsert error: {result.DebugInformation}", true );
                }
            }

            _context.AddMessage( $"Upsert {products.Count} products" );
        }

        private void CheckResponse(
            IResponse response )
        {
            if( response.IsValid ) {
                return;
            }

            _context.AddMessage( response.DebugInformation );
        }

        public long CountDisabledProductsByShop(
            string shopId )
        {
            var result = _client.Count<Product>(
                c => c.Query(
                    q => q.Bool(
                        b => b.Must(
                            term1 => term1.Term( t => t.Field( p => p.ShopId ).Value( shopId ) ),
                            term2 => term2.Term( t => t.Field( p => p.Soldout ).Value( 1 ) ) ) ) ) );
            CheckResponse( result );
            return result.Count;
        }

        public long CountProductsForShop(
            string shopId )
        {
            var result = _client.Count<Product>(
                c => c.Query(
                    q => q.Bool(
                        b => b.Must( term1 => term1.Term( t => t.Field( p => p.ShopId ).Value( shopId ) ) ) ) ) );

            CheckResponse( result );
            return result.Count;

        }

        public ProductPart GetFirstEnableProductByShopIdAndCategoryId( string shopId, string categoryId )
        {
            var response = _client.Search<Product,ProductPart>( s => s.Query( q => q.Bool( b => b.Must(
                must1 => must1.Term( t => t.Field( p => p.ShopId ).Value( shopId ) ),
                must2 => must2.Term( t => t.Field( p => p.OriginalCategoryId ).Value( categoryId ) ),
                must3 => must3.Term( t => t.Field( p => p.Soldout ).Value( 0 ) ) ) ) )
                .Size( 1 ) );
            CheckResponse( response );
            return response.Documents.FirstOrDefault();
        }

    }
}