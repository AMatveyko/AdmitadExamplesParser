// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using AdmitadCommon;
using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Entities.Responses;
using AdmitadCommon.Extensions;
using AdmitadCommon.Helpers;

using Common.Entities;
using Common.Settings;

using Elasticsearch.Net;

using Nest;

using NLog;

namespace Admitad.Converters.Workers
{
    public sealed class ElasticSearchClient< T > : BaseComponent, IElasticClient<T>
        where T : class, IIndexedEntities
    {

        private static readonly Logger Logger = LogManager.GetLogger( "ErrorLogger" );
        
        private const ComponentType Type = ComponentType.ElasticSearch;
        private ElasticClient _client;
        private bool _withStatistics;
        private int _frameSize;
        private readonly ElasticSearchClientSettings _settings;
        private Queue<T> _failedItems = new();
        private const string CategoryIdsFiels = "categoryIds";
        private const string TagsField = "tags";

        public ElasticSearchClient(
            ElasticSearchClientSettings settings,
            BackgroundBaseContext context,
            bool withStatics = false )
            : base( ComponentType.ElasticSearch, context )
        {
            var clientSettings = new ConnectionSettings( new Uri( settings.ElasticSearchUrl ) )
                //.Proxy(new Uri( "http://127.0.0.1:8888" ), string.Empty, string.Empty )
                .RequestTimeout( TimeSpan.FromMinutes( 10 ) ).DefaultIndex( settings.DefaultIndex );
            _client = new ElasticClient( clientSettings );
            _withStatistics = withStatics;
            _frameSize = settings.FrameSize;
            _settings = settings;
            
            Setup();
        }

        private void Setup()
        {
            _client.Cluster.PutSettings( descriptor => descriptor.Transient( f => f.Add( "script.max_compilations_rate", "10000/1m" ) ) );
        }

        #region Scroll api

        public List<string> GetIds()
        {
            var searchResponse = _client.Search<Product>(s => s
                .Source( false ).Query(q => q.MatchAll() )
                .Scroll("1m") 
            );

            var ids = new List<string>();

            while ( searchResponse.Documents.Any() ) {
                ids.AddRange( searchResponse.Hits.Select( p => p.Id ) );
                Console.WriteLine( ids.Count );
                searchResponse = _client.Scroll<Product>("1m", searchResponse.ScrollId );
            }
            
            
            
            return ids;
        }
        
        #endregion
        
        #region Product

        public ProductResponse GetProduct( string id )
        {
            var result = _client.Get<ProductResponse>( id, d => d.Routing( $"R-{id}" ) );
            return result.Source;
        }
        
        #endregion
        
        #region Bulk
        
        public void IndexMany(
            IEnumerable<T> entities )
        {
            MeasureWorkTime( () => DoIndexMany( entities ) );
        }

        private void DoIndexMany(
            IEnumerable<T> entities )
        {
            var entitiesList = entities.ToList();
            var count = entitiesList.Count;
            var frameSize = _frameSize;
            AddStatisticLine( $"Index {entitiesList.Count}" );
            for( var i = 0; i < count; i += frameSize ) {
                var response = _client.IndexMany( entitiesList.Skip( i ).Take( _frameSize ) );
                if( response.DebugInformation.Contains(
                    "Invalid NEST response built from a unsuccessful (429) low level call on POST: /_bulk" ) ) {
                    AddStatisticLine(
                        $"Error: Invalid NEST response built from a unsuccessful (429) low level call on POST: /_bulk, i = {i}, frameSize = {frameSize}" );
                    if( frameSize >= 10000 ) {
                        i -= frameSize;
                        frameSize -= 10000;
                        AddStatisticLine( $"New frame size: {frameSize}" );
                    }
                    else {
                        break;
                    }
                }
            }
        }

        public void Bulk(
            IEnumerable<T> entities )
        {
            MeasureWorkTime( () => DoBulk( entities ) );
        }

        private void DoBulk(
            IEnumerable<T> entities )
        {
            var entitiesList = entities.ToList();
            var count = entitiesList.Count;
            var countUniq = entities.Select( e => e.Id ).Distinct();
            var frameSize = _frameSize;
            AddStatisticLine( $"Index {entitiesList.Count}" );
            for( var i = 0; i < count; i += frameSize ) {
                var bulkIndexResponse = _client.Bulk(
                    b => b.Index( _settings.DefaultIndex ).IndexMany( entitiesList.Skip( i ).Take( _frameSize ) ) );
            }
        }

        public void BulkAll(
            IEnumerable<T> entities )
        {
            MeasureWorkTime( () => DoBulkAll( entities ) );
        }
        
        #endregion

        #region Property
        
        public UpdateResult LinkProductsByProperty( BaseProperty property )
        {

            var field = property.FieldName;
            
            var response = _client.UpdateByQuery<Product>( r =>
                    r.Query( q => q.Bool( b => LinkedPropertyBool( b, property ) ) )
                    .Script( script => script.Source( $"if( ctx._source.{field} == null ){{ ctx._source.{field} = new ArrayList(); }}ctx._source.{field}.add( {property.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response );
        }

        public long GetCountAllDocuments()
        {
            var response = _client.Count<Product>( r => r.Query( q => q.MatchAll() ) );
            return response.Count;
        }
        
        public UpdateResult UnlinkProductsByProperty( BaseProperty property )
        {
            var response = _client.UpdateByQuery<Product>( r =>
                r.Query( q => q.Bool( b => UnlinkedPropertyBool( b, property ) ) )
                    .Script( script => script.Source( $"ctx._source.{property.FieldName}.remove( ctx._source.{property.FieldName}.indexOf( {property.Id} ) );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response );
        }

        public long CountUnlinkProductsByProperty( BaseProperty property )
        {
            return _client.Count<Product>( r => r.Query( q =>
                q.Bool( b => UnlinkedPropertyBool( b, property ) ) ) ).Count;
        }
        
        private static BoolQueryDescriptor<Product> UnlinkedPropertyBool( BoolQueryDescriptor<Product> query, BaseProperty property )
        {
            var namesWithNot = property.Names.Select( n => $"NOT {n}" );
            var queryString = string.Join(" AND ", namesWithNot );
            
            var boolQuery = query.Filter(
                f => f.QueryString(
                    qs => qs.Fields( f1 => GetFields( f1, property.FieldsForSearch.ToArray() ) )
                        .Query( queryString ) ) );
            
            boolQuery = boolQuery.Must(
                bm => bm.Term( bmt => bmt.Field( property.FieldName ).Value( property.Id ) ) );

            return boolQuery;

        }
        
        private static BoolQueryDescriptor<Product> LinkedPropertyBool(
            BoolQueryDescriptor<Product> query, BaseProperty property )
        {

            var queryString = string.Join(" OR ", property.SearchNames );

            
            
            var boolQuery = query.Filter(
                f => f.QueryString(
                    qs => qs.Fields( f => GetFields( f, property.FieldsForSearch.ToArray() ) )
                        .Query( queryString ) ) );

            // может и нет... //Нужно каждый раз перепривязывать совйства т.к. старых свойств по которым уже есть привяка может не быть после обновления
            boolQuery = boolQuery.MustNot(
                bm => bm.Term( bmt => bmt.Field( property.FieldName ).Value( property.Id ) ) );
            
            return boolQuery;
        }
        #endregion
        
        #region Brand

        public long GetCountWithClearlyName( string clearlyName, string brandId = Constants.UndefinedBrandId )
        {
            var terms = new List<Func<QueryContainerDescriptor<Product>,QueryContainer>>();
            terms.Add( f => f.Term( t => t.Field( p => p.VendorNameClearly ).Value( clearlyName ) ) );
            if( brandId != Constants.UndefinedBrandId ) {
                terms.Add( f => f.Term( t => t.Field( p => p.BrandId ).Value( brandId ) ) );
            }
            var response = _client.Count<Product>( c => c.Query( q => q.Bool( b => b.Filter( terms ) ) ) );
            return response.Count;
        }

        public UpdateResult UpdateBrandId( string clearlyName, string brandId )
        {
            var response = _client.UpdateByQuery<Product>( upq =>
                upq.Query( q => q.Bool( 
                        b => b.Must( m => m.Term( t => t.Field( p => p.VendorNameClearly ).Value( clearlyName ) ) )
                            .MustNot( mn => mn.Term( t => t.Field( p => p.BrandId ).Value( brandId ) ) )
                        ) )
                    .Script( script => script.Source( $"ctx._source.brandId = {brandId}" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response.Total, response.Updated );
        }
        
        #endregion

        #region Countries

        public UpdateResult UpdateProductsFroCountry(
            Country country )
        {
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQuery( b, country ) ) )
                    .Script( script => script.Source( $"ctx._source.countryId = { country.Id }" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response );
        }

        private static BoolQueryDescriptor<Product> BuildBoolQuery(
            BoolQueryDescriptor<Product> descriptor,
            Country country )
        {
            var query = string.Join( " OR ", country.SearchTerms );
            descriptor = descriptor.Filter(
                queryString => queryString.QueryString(
                    qs => qs.Query( query ) ),
                q => q.Term( t => t.Field( "countryId.keyword" ).Value( Constants.UndefinedCountryId.ToString() ) ) );
            return descriptor;
        } 
        
        #endregion
        
        #region Tags
        public UpdateResult UpdateProductsForTag( Tag tag ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQuery( b, tag ) ) )
                    .Script( script => script.Source( $"if( ctx._source.tags == null ){{ ctx._source.tags = new ArrayList(); }}ctx._source.tags.add( {tag.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response.Total, response.Updated );

        }

        public long CountProductsForTag( Tag tag )
        {
            var result = _client.Count<Product>( r => r.Query( q => q.Bool( b => BuildBoolQuery( b, tag ) ) ) );
            return result.Count;
        }

        public long CountProductsWithTag( string tagId )
        {
            var result = _client.Count<Product>( r => r.Query( q => q.Bool( b => b.Filter(
                f => f.Term( t => t.Field( c => c.Tags ).Value( tagId ) ) ))) );
            return result.Count;
        }
        
        private static BoolQueryDescriptor<Product> BuildBoolQuery(
            BoolQueryDescriptor<Product> descriptor,
            Tag tag )
        {
            var query = $"( { string.Join( " OR ", tag.SearchTerms ) } )";

            if( tag.ExcludePhrase.Any() ) {
                var queryExcludePhrase = string.Join( " AND ", tag.ExcludePhrase.Select( ep => $"NOT {ep}" ) );
                query = $"{query} AND ( {queryExcludePhrase} )";
            }

            if( tag.SpecifyWords != null &&
                tag.SpecifyWords.Any() ) {
                var specifyQuery = string.Join( " OR ", tag.SpecifyWords );
                query += $" AND ( {specifyQuery} )";
            }
            
            descriptor = descriptor.Filter( 
                queryString => queryString.QueryString( qs => qs.Fields( fields => GetFields( fields, tag.Fields)).Query( query ) ),
                //m => m.Term( t => t.Field( ld => ld.Categories ).Value( tag.IdCategory ) )
                //m => m.LongRange( r => r.Field( "categories" ).GreaterThanOrEquals( tag.IdCategory ).LessThanOrEquals( CategoryHelper.GetEndCategory( tag.IdCategory ) ) )
                m => m.Terms( t => t.Field( "categories" ).Terms( tag.Categories ) )
                 );

            descriptor =
                descriptor.MustNot( mn => mn.Term( t => t.Field( ld => ld.Tags ).Value( tag.Id ) ) );
            
            return descriptor;
        }
        
        public UpdateResult UnlinkTag( Tag tag )
        {
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => GetBoolProductsWithTag( b, tag) ) )
                    .Script( script => script.Source( $"ctx._source.tags.remove( ctx._source.tags.indexOf( { tag.Id } ) );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response.Total, response.Updated );
        }
        
        private static BoolQueryDescriptor<Product> GetBoolProductsWithTag(
            BoolQueryDescriptor<Product> boolQuery,
            Tag tag )
        {
            boolQuery = boolQuery.Filter( f => f.Term( t => t.Field( c => c.Tags ).Value( tag.Id ) ) );
            return boolQuery;
        }
        
        #endregion
        
        #region Categories

        public UpdateResult UnlinkCategory( Category category )
        {
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => GetBoolProductsWithCategory( b, category.Id) ) )
                    .Script( script => script.Source( $"ctx._source.categories.remove( ctx._source.categories.indexOf( { category.Id } ) );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response.Total, response.Updated );
        }
        
        public UpdateResult UpdateProductsForCategory( Category category ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQuery( b, category ) ) )
                    .Script( script => script.Source( $"if( ctx._source.categories == null ){{ ctx._source.categories = new ArrayList(); }}ctx._source.categories.add( {category.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response );
        }

        public UpdateResult UpdateProductsForCategoryFieldNameModel( Category category ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQueryFieldNameModel( b, category ) ) )
                    .Script( script => script.Source( $"if( ctx._source.categories == null ){{ ctx._source.categories = new ArrayList(); }}ctx._source.categories.add( {category.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response.Total, response.Updated );
        }
        
        public UpdateResult UpdateProductsForCategoryNew( Category category ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQueryCategoriesNew( b, category ) ) )
                    .Script( script => script.Source( $"ctx._source.categories = {category.Id};" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response );
        }

        private static string GetResult(
            UpdateByQueryResponse response ) =>
            $"{response.Total}/{response.Updated}"; 
        
        public long CountProductsForCategory( Category category )
        {
            var result = _client.Count<Product>( r => r.Query( q => q.Bool( b => BuildBoolQuery( b, category ) ) ) );
            return result.Count;
        }
        
        public long CountProductsForCategoryNew( Category category )
        {
            var result = _client.Count<Product>( r => r.Query( q => q.Bool( b => BuildBoolQueryCategoriesNew( b, category ) ) ) );
            return result.Count;
        }
        
        public long CountProductsForCategoryFieldNameModel( Category category )
        {
            var result = _client.Count<Product>( r => r.Query( q => q.Bool( b => BuildBoolQueryFieldNameModel( b, category ) ) ) );
            return result.Count;
        }
        
        public long CountProductsWithCategory( string categoryId )
        {
            var result = _client.Count<Product>( r =>
                r.Query( q => q.Bool( b => GetBoolProductsWithCategory( b, categoryId ) ) ) );
            return result.Count;
        }

        private static BoolQueryDescriptor<Product> GetBoolProductsWithCategory(
            BoolQueryDescriptor<Product> boolQuery,
            string categoryId )
        {
             boolQuery = boolQuery.Filter( f => f.Term( t => t.Field( c => c.Categories ).Value( categoryId ) ) );
             return boolQuery;
        }
        
        private static BoolQueryDescriptor<Product> BuildBoolQueryFieldNameModel(
            BoolQueryDescriptor<Product> descriptor,
            Category category )
        {
            var query = string.Join( " OR ", category.Terms );
            if( category.SearchSpecify != null &&
                category.SearchSpecify.Any() ) {
                var queryWithSpecify = string.Join( " OR ", category.SearchSpecify );
                query = $"( {query} ) AND ( {queryWithSpecify} )";
            }

            var criteria = new List<Func<QueryContainerDescriptor<Product>,QueryContainer>> {
                gbs => gbs.Term( t => t.Field( p => p.Gender ).Value( category.Gender ) )
            };

            if( category.TakeUnisex ) {
                criteria.Add( gbs => gbs.Term( t => t.Field( p => p.Gender ).Value( GenderHelper.Convert( Gender.Unisex ) ) ) );
            }
            
            var terms = new List<Func<QueryContainerDescriptor<Product>,QueryContainer>> {
                queryString => queryString.QueryString( qs => qs.Fields( fields => GetFields( fields, category.Fields )).Query( query ) ),
                gender => gender.Bool( gb => gb.Should( criteria ) ),
                age => age.Term( t => t.Field( p => p.Age ).Value( category.Age ) )
            };
            
            if( category.ExcludeTerms != null &&
                category.ExcludeTerms.Any() ) {
                var queryForExcludeWords = string.Join( " AND ", category.ExcludeTerms.Select( et => $"NOT {et}" ) );
                terms.Add( queryString => queryString.QueryString( qs => qs.Fields( fields => GetFields( fields, category.ExcludeWordsFields )).Query( queryForExcludeWords ) ) );
            }
            
            descriptor = descriptor.Filter( terms );

            descriptor =
                descriptor.MustNot( mn => mn.Term( t => t.Field( ld => ld.Categories ).Value( category.Id ) ) );
            
            return descriptor;
        }
        
        private static BoolQueryDescriptor<Product> BuildBoolQuery(
            BoolQueryDescriptor<Product> descriptor,
            Category category )
        {
            const string queryWithExcludeWordsPattern = "( {0} ) AND ( {1} )";
            var query = string.Join( " OR ", category.Terms );
            if( category.ExcludeTerms != null &&
                category.ExcludeTerms.Any() ) {
                var queryForExcludeWords = string.Join( " AND ", category.ExcludeTerms.Select( et => $"NOT {et}" ) );
                query = string.Format( queryWithExcludeWordsPattern, query, queryForExcludeWords );
            }

            descriptor = descriptor.Filter( queryString => queryString.QueryString( qs => qs.Fields( fields => GetFields( fields, category.Fields)).Query( query ) ),
                gender => gender.Term( t => t.Field( p => p.Gender ).Value( category.Gender ) ),
                age => age.Term( t => t.Field( p => p.Age ).Value( category.Age ) ) );

            descriptor =
                descriptor.MustNot( mn => mn.Term( t => t.Field( ld => ld.Categories ).Value( category.Id ) ) );
            
            return descriptor;
        }
        
        private static BoolQueryDescriptor<Product> BuildBoolQueryCategoriesNew(
            BoolQueryDescriptor<Product> descriptor,
            Category category )
        {
            const string queryWithExcludeWordsPattern = "( {0} ) AND ( {1} )";
            var query = string.Join( " OR ", category.Terms );
            if( category.ExcludeTerms != null &&
                category.ExcludeTerms.Any() ) {
                var queryForExcludeWords = string.Join( " AND ", category.ExcludeTerms.Select( et => $"NOT {et}" ) );
                query = string.Format( queryWithExcludeWordsPattern, query, queryForExcludeWords );
            }

            descriptor = descriptor.Filter( queryString => queryString.QueryString( qs => qs.Fields( fields => GetFields( fields, category.Fields)).Query( query ) ),
                gender => gender.Term( t => t.Field( p => p.Gender ).Value( category.Gender ) ),
                age => age.Term( t => t.Field( p => p.Age ).Value( category.Age ) ) );

            descriptor =
                descriptor.MustNot( mn => mn.Exists( e => e.Field( "categories" ) ) );
            
            return descriptor;
        }
        
        #endregion

        #region Routin
        
        private static IEnumerable<Func<QueryContainerDescriptor<Product>, QueryContainer>> GetTerms( string[] fields, string[] terms ) {
            foreach( var field in fields ) {
                yield return m => m.Terms( t => t.Field( field ).Terms( terms ) );
            }
        }
        
        private static FieldsDescriptor<Product> GetFields( FieldsDescriptor<Product> descriptor, string[] fields )
        {
            return fields.Aggregate( descriptor, ( current, field ) => current.Field( field ) );
        }
        
        
        
        #endregion

        #region Upload products and LinkedData
        
        public void BulkLinkedData( List<LinkedData> data )
        {
            var result = _client.Bulk( b => b.CreateMany( data, (
                descriptor,
                linkedData ) => descriptor.Routing( linkedData.RoutingId ) ) );
            ResponseCollector.Responses.Add( ( _settings.ShopName, "ld", result ) );
        }
        
        public void DoBulkAllInsert(
            IEnumerable<T> entities )
        {
            var list = entities.ToList();
            var position = 0;
            var frameSize = 300000;
            while( position < list.Count ) {
                var portion = list.Skip( position ).Take( frameSize );
                var result = _client.Bulk( b => b.IndexMany( portion, (
                    descriptor,
                    entity ) => descriptor.Routing( entity.RoutingId ) ) );
                ResponseCollector.Responses.Add( ( _settings.ShopName, "p", result ) );
                position += frameSize;
            }
        }
        
        public void DoBulkAll(
            IEnumerable<T> entities )
        {
            var list = entities.ToList();
            var position = 0;
            var frameSize = 100000;
            var count = 0;
            while( position < list.Count ) {
                var portion = list.Skip( position ).Take( frameSize );
                var result = _client.Bulk( b => b.UpdateMany<T,IProductForIndex>( portion, (
                    descriptor,
                    entity ) => descriptor.Upsert( entity ).DocAsUpsert().Doc( (IProductForIndex)entity ).Routing( entity.RoutingId ) ) );
                ResponseCollector.Responses.Add( ( _settings.ShopName, "products", result ) );
                position += frameSize;
                count += portion.Count();
                if( result.DebugInformation.Contains( "Invalid" ) ) {
                    _context.AddMessage( "Update error!", true );
                    Logger.Error( result.DebugInformation );
                }
                
            }
            _context.AddMessage( $"Update { count } products" );
        }

        public void DoBulkAllForImport(
            IEnumerable<T> entities )
        {
            var list = entities.ToList();
            var position = 0;
            var frameSize = 100000;
            var count = 0;
            while( position < list.Count ) {
                var portion = list.Skip( position ).Take( frameSize );
                var result = _client.Bulk( b => b.UpdateMany<T,T>( portion, (
                    descriptor,
                    entity ) => descriptor.Upsert( entity ).DocAsUpsert().Doc( (T)entity ).Routing( entity.RoutingId ) ) );
                ResponseCollector.Responses.Add( ( _settings.ShopName, "products", result ) );
                position += frameSize;
                count += portion.Count();
                if( result.DebugInformation.Contains( "Invalid" ) ) {
                    _context.AddMessage( "Update error!", true );
                    Logger.Error( result.DebugInformation );
                }
                
            }
            _context.AddMessage( $"Update { count } products" );
        }
        
        public void DoBulkAllOld(
            IEnumerable<T> entities )
        {
            
            var entitiesList = entities.ToList();
            var count = entitiesList.Count;
            var countUniq = entities.Select( e => e.Id ).Distinct();
            
            AddStatisticLine( $"Count: {count}" );
            AddStatisticLine( $"Uniq: {countUniq}" );
            
            var bulkAllObservable = _client.BulkAll(
                entities, b =>
                    b.Index( _settings.DefaultIndex )
                        .BackOffTime( "3s" )
                        .BackOffRetries( 10 )
                        .RefreshOnCompleted()
                        .MaxDegreeOfParallelism( 6 )
                        .Size( 20000 )
                        .ContinueAfterDroppedDocuments()
                        .DroppedDocumentCallback( DroppedDocumentCallback ) );
            var handler = new Handler();
            var bulkAllObserver = new BulkAllObserver(
                onNext: response => Console.Write( $"{response.Page} " ),
                onError: response => Console.Write( "E " ),
                onCompleted: () => {
                    LogWriter.Log( "Complete!" );
                    handler.Complete();
                } );
            bulkAllObservable.Subscribe( bulkAllObserver );
            while( handler.IsWait() ) {
                Thread.Sleep( 5000 );
            }
        }

        private void DroppedDocumentCallback(
            BulkResponseItemBase response,
            T item )
        {
            _failedItems.Enqueue( item );
        }
        
        private class Handler
        {
            private bool _flag = true;
            public void Complete() => _flag = false;
            public bool IsWait() => _flag;
        }
        
        #endregion




        public UpdateResult UpdateAddDate( DateTime? date = null )
        {
            var newDate = date ?? DateTime.Now;
            var response = _client.UpdateByQuery<Product>(
                upq => upq.Query( q =>
                        q.Bool( b =>
                            b.MustNot( t =>
                                t.Exists( p =>
                                    p.Field( "addDate" ) 
                                )
                            )
                        )
                    )
                    .Script( script => script.Source( $"ctx._source.addDate = '{ newDate :O}'" ) )
                );
            return new UpdateResult( response );
        }
            
        #region Disable/Enable documetns
        
        public UpdateResult DisableOldProducts( DateTime indexTime, string shopId ) {
            var response = _client.UpdateByQuery<Product>( upq =>
                upq.Query( q => q.Bool( qb =>
                        GetBoolForDisableProducts( qb, indexTime, shopId ) ) )
                    .Script( script => script.Source( "ctx._source.soldout = 1" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return new UpdateResult( response );
        }

        public long CountDisabledProductsByShop( string shopId )
        {
            var result = _client.Count<Product>( c => c.Query( q => q.Bool( b => b.Must(
                m => m.Term( t => t.Field( p => p.ShopId ).Value( shopId ) ),
                m => m.Term( t => t.Field( p => p.Soldout ).Value( 1 ) ) ) ) ) );
            return result.Count;
        }
        
        public long CountForDisableProducts( DateTime indexTime, string shopId )
        {
            return _client
                .Count<Product>( c => c.Query( q => q.Bool( b => GetBoolForDisableProducts( b, indexTime, shopId ) ) ) )
                .Count;
        }

        private static BoolQueryDescriptor<Product> GetBoolForDisableProducts( BoolQueryDescriptor<Product> boolQuery, DateTime indexTime, string shopId )
        {
            var terms = new List<Func<QueryContainerDescriptor<Product>, QueryContainer>> {
                m => m.DateRange( range => range.Field( p => p.UpdateDate ).LessThan( indexTime ) )
            };

            if( shopId.IsNotNullOrWhiteSpace() ) {
                terms.Add( m => m.Term( t => t.Field( p => p.ShopId ).Value( shopId ) ) );
            }
            
            boolQuery = boolQuery.Filter( f => f.Bool( b =>
                b.Must( terms )
                    .MustNot( mn => mn.Term( qbft => qbft.Field( ld => ld.Soldout ).Value( 1 ) ) ) ) );
            return boolQuery;
        }

        public long CountProducts()
        {
            var response = _client.Count<Product>( c => c.Query( q => q.MatchAll() ) );
            return response.Count;
        }

        public long CountSoldOutProducts()
        {
            var response = _client.Count<Product>(
                c => c.Query( q => q.Term( t => t.Field( p => p.Soldout ).Value( 1 ) ) ) );
            return response.Count;
        }
        
        #endregion
        
        
        
        public long CountProductsForShop( string shopId )
        {
            var result = _client.Count<Product>( c => 
                c.Query( q => 
                    q.Term( t => t.Field( p => p.ShopId ).Value( shopId ) ) ) );
            return result.Count;
        }
        
    }
}