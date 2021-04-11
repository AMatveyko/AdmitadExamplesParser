// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadExamplesParser.Entities;

using Elasticsearch.Net;

using Nest;

namespace AdmitadExamplesParser.Workers.Components
{
    public sealed class ElasticSearchClient< T > : BaseComponent, IElasticClient<T>
        where T : class, IIndexedEntities
    {

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
            bool withStatics = false )
            : base( ComponentType.ElasticSearch )
        {
            var clientSettings = new ConnectionSettings( new Uri( settings.ElasticSearchUrl ) )
                .RequestTimeout( TimeSpan.FromMinutes( 10 ) ).DefaultIndex( settings.DefaultIndex );
            _client = new ElasticClient( clientSettings );
            _withStatistics = withStatics;
            _frameSize = settings.FrameSize;
            _settings = settings;
        }

        public void IndexMany(
            IEnumerable<T> entities )
        {
            MeasureWorkTime( () => DoIndexMany( entities ) );
        }

        private void DoIndexMany(
            IEnumerable<T> entities )
        {
            var entitiesList = entities.ToList();
            var valid = 0;
            var invalid = 0;
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




        #region ScrollApi

        public HashSet<string> GetIdsUnlinkedProductsByScroll()
        {
            var ids = new HashSet<string>();
            try {
                var serchResponce = _client.Search<Product>(
                    s => s.Query( q => q.Exists( e => e.Field( "categories" ) ) ).Scroll( "10s" ) );
                while( serchResponce.Documents.Any() ) {
                    foreach( var document in serchResponce.Documents ) {
                        ids.Add( document.Id );
                    }
                }
            }
            catch( Exception e ) {
                ;
            }

            

            return ids;
        }

        #endregion


        #region Property
        
        public string LinkProductsByProperty( BaseProperty property )
        {

            var field = property.FieldName;
            
            var response = _client.UpdateByQuery<Product>( r =>
                    r.Query( q => q.Bool( b => LinkedPropertyBool( b, property ) ) )
                    .Script( script => script.Source( $"if( ctx._source.{field} == null ){{ ctx._source.{field} = new ArrayList(); }}ctx._source.{field}.add( {property.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return GetResult( response );
        }

        public long GetCountAllDocuments()
        {
            var response = _client.Count<Product>( r => r.Query( q => q.MatchAll() ) );
            return response.Count;
        }
        
        public string UnlinkProductsByProperty( BaseProperty property )
        {
            var response = _client.UpdateByQuery<Product>( r =>
                r.Query( q => q.Bool( b => UnlinkedPropertyBool( b, property ) ) )
                    .Script( script => script.Source( $"ctx._source.{property.FieldName}.remove( ctx._source.{property.FieldName}.indexOf( {property.Id} ) );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return GetResult( response );
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

            var queryString = string.Join(" OR ", property.Names );

            
            
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

        #region Tags
        public string UpdateProductsForTag( Tag tag ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQuery( b, tag ) ) )
                    .Script( script => script.Source( $"if( ctx._source.tags == null ){{ ctx._source.tags = new ArrayList(); }}ctx._source.tags.add( {tag.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return GetResult( response );

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
            var query = string.Join( " OR ", tag.SearchTerms );

            descriptor = descriptor.Filter( 
                queryString => queryString.QueryString( qs => qs.Fields( fields => GetFields( fields, tag.Fields)).Query( query ) ),
                gender => gender.Term( t => t.Field( p => p.Gender ).Value( tag.Gender ) ) );

            descriptor =
                descriptor.Must( m => m.Term( t => t.Field( ld => ld.Categories ).Value( tag.IdCategory ) ) );
            descriptor =
                descriptor.MustNot( mn => mn.Term( t => t.Field( ld => ld.Tags ).Value( tag.Id ) ) );
            
            return descriptor;
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
        
        public string UpdateProductsForCategory( Category category ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQuery( b, category ) ) )
                    .Script( script => script.Source( $"if( ctx._source.categories == null ){{ ctx._source.categories = new ArrayList(); }}ctx._source.categories.add( {category.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return GetResult( response );
        }

        public string UpdateProductsForCategoryFieldNameModel( Category category ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQueryFieldNameModel( b, category ) ) )
                    .Script( script => script.Source( $"if( ctx._source.categories == null ){{ ctx._source.categories = new ArrayList(); }}ctx._source.categories.add( {category.Id} );" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return GetResult( response );
        }
        
        public string UpdateProductsForCategoryNew( Category category ) {
            
            var response = _client.UpdateByQuery<Product>(
                r => r.Query( q => q.Bool( b => BuildBoolQueryCategoriesNew( b, category ) ) )
                    .Script( script => script.Source( $"ctx._source.categories = {category.Id};" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return GetResult( response );
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
            var terms = new List<Func<QueryContainerDescriptor<Product>,QueryContainer>> {
                queryString => queryString.QueryString( qs => qs.Fields( fields => GetFields( fields, new [] { "name", "model" } )).Query( query ) ),
                gender => gender.Term( t => t.Field( p => p.Gender ).Value( category.Gender ) ),
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

        #region Upload products and linked data
        
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
            while( position < list.Count ) {
                var portion = list.Skip( position ).Take( frameSize );
                var result = _client.Bulk( b => b.UpdateMany<T,IProductForIndex>( portion, (
                    descriptor,
                    entity ) => descriptor.Upsert( entity ).DocAsUpsert().Doc( (IProductForIndex)entity ).Routing( entity.RoutingId ) ) );
                ResponseCollector.Responses.Add( ( _settings.ShopName, "products", result ) );
                position += frameSize;
            }
        }

        private void DoBulkAllOld(
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
        
        #region Disable/Enable documetns
        
        public string DisableOldProducts( DateTime indexTime ) {
            
            var response = _client.UpdateByQuery<Product>( upq =>
                upq.Query( q => q.Bool( qb =>
                        GetBoolForDisableProducts( qb, indexTime ) ) )
                    .Script( script => script.Source( "ctx._source.soldout = 1" ) )
                    .Conflicts( Conflicts.Proceed )
                    .Refresh( true ) );
            return GetResult( response );
        }

        public long CountDisabledProducts( DateTime indexTime )
        {
            return _client
                .Count<Product>( c => c.Query( q => q.Bool( b => GetBoolForDisableProducts( b, indexTime ) ) ) )
                .Count;
        }
        
        private static BoolQueryDescriptor<Product> GetBoolForDisableProducts( BoolQueryDescriptor<Product> boolQuery, DateTime indexTime )
        {
            boolQuery = boolQuery.Filter( f => f.Bool( b =>
                b.Must( m => m.DateRange( range => range.Field( p => p.UpdateDate ).LessThan( indexTime ) ) )
                    .MustNot( mn => mn.Term( qbft => qbft.Field( ld => ld.Soldout ).Value( 1 ) ) ) ) );
            return boolQuery;
        }
        
        #endregion
        
    }
}