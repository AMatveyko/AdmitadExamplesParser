// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Api;
using Common.Elastic.Entities;
using Common.Elastic.Extensions;
using Common.Entities;
using Common.Extensions;
using Common.Settings;

using Elasticsearch.Net;

using Nest;

namespace Common.Elastic.Workers
{
    public sealed class IndexClient : IIndexClient, IIndexProductWorker, IIndexTagsWorker
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
                new ConnectionSettings( new Uri( settings.ElasticSearchUrl ) )
                .RequestTimeout(TimeSpan.FromMinutes(20))
                .DefaultIndex( settings.DefaultIndex );
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
        
        public static IIndexClient CreateIndexClient(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context ) =>
            Create( settings, context );


        public static IIndexProductWorker CreateProductWorker(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context ) =>
            Create( settings, context );

        public static IIndexTagsWorker CreateTagsWorker(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context ) =>
            Create( settings, context );

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

        private List<T> GetItemsByScrolling<T>(
            string scrollingTime,
            Func<QueryContainerDescriptor<T>,QueryContainer> query,
            Func<SourceFilterDescriptor<T>, ISourceFilter> selector = null,
            int size = 1000 ) where T : class {

            var searchResponce = _client.Search<T>(s => {
                s.Size(size).Query(query).Scroll(scrollingTime);
                if( selector != null) {
                    s.Source(selector);
                }
                return s;
            });

            CheckResponse(searchResponce);
            
            var items = new List<T>();

            _context.AddMessage($"Suitable products {searchResponce.Total}");

            // var counts = 0;

            while (searchResponce.Documents.Any()) {
                // counts++;
                items.AddRange(searchResponce.Documents);
                searchResponce = _client.Scroll<T>(scrollingTime, searchResponce.ScrollId);
                CheckResponse(searchResponce);
                //  sdfsfsdfsdf
                // if( counts == 6) {
                //     break;
                // }
            }

            _context.AddMessage($"Received products {items.Count}");

            return items;

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
                        (   descriptor,
                            entity ) => descriptor.Upsert( entity ).DocAsUpsert().Doc( ( IProductForIndex ) entity )
                            .Routing( entity.RoutingId ) ) );
                position += _settings.FrameSize;
                if( result.IsValid == false ) {
                    _context.AddMessage( $"Upsert error: {result.DebugInformation}", true );
                }
            }

            _context.AddMessage( $"Upsert {products.Count} products" );
        }

        #region UpdateRating

        public List<ProductRatingInfoFromElastic> GetProductsForUpdatingRating() {

            const string fieldName = "ratingUpdateDate";

            var query = new Func<QueryContainerDescriptor<ProductRatingInfoFromElastic>, QueryContainer>(
                q => q.Bool( b => b.Filter( 
                    f => f.Exists( fe => fe.Field("categories") ),
                    f => f.Bool(fb =>
                                    fb.Should(
                                        s1 => s1.DateRange(r => r.Field(fieldName).LessThan(DateTime.Today)),
                                        // s1 => s1.DateRange(r => r.Field(fieldName).LessThan(DateTime.Now)),
                                        s2 => s2.Bool(b => b.MustNot(mn => mn.Exists(e => e.Field(fieldName))))
                                )
                        )
                    ) )
            );

            return GetItemsByScrolling("10m",query, i => i.Includes( inc => inc.Fields( new[] { "id", "soldout", "shopId", "rating" })) ,10000);

        }

        public void UpdateProductRating(List<Product> products) {

            var position = 0;
            while (position < products.Count)
            {
                var portion = products.Skip(position).Take( _settings.FrameSize );
                var result = _client.Bulk(
                    b => b.UpdateMany<Product, IProductRatingUpdateData>( portion, (selector, item) => selector.Routing(item.RoutingId).Doc((IProductRatingUpdateData)item) ) );
                position += _settings.FrameSize;
                if (result.IsValid == false) {
                    _context.AddMessage($"Update error: {result.DebugInformation}", true);
                }
            }

            _context.AddMessage($"Updated {products.Count} products");
        }

        #endregion

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

        public async Task<IProductPhotos> GetProductsPhotoAsync(
            string id,
            string index = null )
        {
            return await Task.Run(
                () => {
                    var result = index.IsNotNullOrWhiteSpace()
                        ? _client.Get<ProductPart>(
                            id,
                            descriptor => descriptor.Index( index ).Routing( ProductPart.GetRouting( id ) ) )
                        : _client.Get<ProductPart>(
                            id,
                            descriptor => descriptor.Routing( ProductPart.GetRouting( id ) ) );
                    return (IProductPhotos)result.Source;
                } );
        }

        public void RemoveTag( string productId, string tagId )
        {

            var properties = new SearchProperties {
                Must = new SearchTerms {
                    TagId = tagId,
                    ProductId = productId
                }
            };

            var result =
                _client.UpdateByQuery<Product>(
                ubq => ubq.Query( q => q.Bool( b =>
                   b.FromSearchParameters( properties ) ) )
                    .Script( script => script.RemoveTag( tagId ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh() );
        } 
        
        public void RemoveCategory( string productId, string categoryId ) {
            
            var properties = new SearchProperties {
                Must = new SearchTerms {
                    CategoryId = categoryId,
                    ProductId = productId
                }
            };
            var result = 
                _client.UpdateByQuery<Product>( ubq => ubq.Query( q => q.Bool( b =>  
                b.FromSearchParameters( properties ) ) )
                .Script( script => script.RemoveCategory( categoryId ) )
                .Conflicts( Conflicts.Proceed )
                .Refresh( true ) );
            
            
        }
                
        private static IndexClient Create(  ElasticSearchClientSettings settings, BackgroundBaseContext context  )
            => new IndexClient( settings, context );

        public long GetProductsCountWithTag( string tagId ) {
            var result = _client.Count<Product>( s => s.Query( q => q.Term( t => t.Field( "tags" ).Value( tagId ) ) ) ).Count;
            return result;
        } 
            
    }
}