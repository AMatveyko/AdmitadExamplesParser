// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters
{
    public sealed class ProductLinker : BaseComponent
    {

        private static readonly Dictionary<string, List<int>> CategoryByProducts = new();
        private readonly IElasticClient<Product> _elasticClient;

        public ProductLinker( ElasticSearchClientSettings settings, BackgroundBaseContext context )
            : base( ComponentType.ProductLinker, context ) {
            _elasticClient = new ElasticSearchClient<Product>( settings, context );
        }




        #region Category
        public void LinkCategories( IEnumerable<Category> categories )
        {
            MeasureWorkTime( () => DoCategoryLink( categories ) );
        }

        private void DoCategoryLink( IEnumerable<Category> categories )
        {
            _context.TotalActions = categories.Count();
            var results = categories.Select( LinkCategory ).OrderByDescending( c => c.Item2.Updated );
            foreach( var (id, count, time) in results ) {
                Log( "category", id, count.Pretty, time.ToString() );
            }
        }

        public void RelinkCategory( Category category )
        {
            var before = ( int ) _elasticClient.CountProductsWithCategory( category.Id );
            var unlinkResult = _elasticClient.UnlinkCategory( category );
            _context.Messages.Add( $"Отвязали { unlinkResult.Pretty } товаров" );
            _context.TotalActions = 2;
            _context.CalculatePercent();

            if( category.IsTermsEmpty() ) {
                _context.CalculatePercent();
                _context.Content = $"{category.Id}: отвязали {unlinkResult.Pretty}";
                DbHelper.UpdateProductsByCategory( category, before, 0 );
                return;
            }
            
            var linkResult = LinkCategory( category );
            _context.Messages.Add( $"Привязвали { linkResult.Item2.Pretty } товаров" );
            _context.Content = $"{category.Id}: отвязали {unlinkResult.Pretty}, привязали {linkResult.Item2.Pretty}, разница { unlinkResult.GetDifferencePercent( linkResult.Item2 ) }%";
            DbHelper.UpdateProductsByCategory( category, before, (int)linkResult.Item2.Updated );
        }
        
        public ( string, UpdateResult, long ) LinkCategory( Category category )
        {
            if( category.IsTermsEmpty() ) {
                _context.CalculatePercent();
                return ( "Empty terms", new UpdateResult( 0, 0 ), 0 );
            }
            var before = ( int ) _elasticClient.CountProductsWithCategory( category.Id );
            var result = Measure( () => _elasticClient.UpdateProductsForCategoryFieldNameModel( category ), out var time );
            var after = (int)_elasticClient.CountProductsWithCategory( category.Id );
            
            DbHelper.UpdateProductsByCategory( category, before, after );
            // var copeunt = Measure( () => _elasticClient.UpdateProductsForCategory( category ), out var time );
            
            _context.CalculatePercent();

            if( result.IsError ) {
                _context.AddMessage( $"id {category.Id} updated { result.Pretty }", result.IsError );
            }
            
            return ( category.Id, result, time );
        }
        #endregion
        
        #region Tag
        public void LinkTags( IEnumerable<Tag> tags ) {
            MeasureWorkTime( () => DoLinkTags( tags ) );
        }

        private void DoLinkTags( IEnumerable<Tag> tags )
        {

            _context.TotalActions = tags.Count();
            
            var results = tags.Select( LinkTag ).OrderByDescending( t => t.Item2.Updated );
            foreach( var (id, count, time) in results ) {
                Log( "tag", id, count.Pretty, time.ToString() );
            }
        }

        public void RelinkTag( Tag tag )
        {
            var unlinkResult = _elasticClient.UnlinkTag( tag );
            _context.Messages.Add( $"Отвязали { unlinkResult.Pretty } товаров" );
            _context.TotalActions = 2;
            if( tag.SearchTerms == null ||
                tag.SearchTerms.Any() == false ) {
                _context.CalculatePercent();
                _context.Content = $"{tag.Id}: отвязали {unlinkResult.Pretty}";
                return;
            }

            var linkResult = _elasticClient.UpdateProductsForTag( tag );
            _context.Messages.Add( $"Привязвали { linkResult.Pretty } товаров" );
            _context.Content = $"{tag.Id}: отвязали {unlinkResult.Pretty}, привязали {linkResult.Pretty}, разница { unlinkResult.GetDifferencePercent( linkResult ) }%";
        }
        
        private ( string, UpdateResult, long ) LinkTag( Tag tag )
        {
            if( tag.IsSearchTermsEmpty() ) {
                _context.CalculatePercent();
                return ( "Empty terms", new UpdateResult( 0, 0 ), 0 );
            }
            var result = Measure( () => _elasticClient.UpdateProductsForTag( tag ), out var time );
            _context.CalculatePercent();
            
            if( result.IsError ) {
                _context.AddMessage( $"id {tag.Id} updated { result.Pretty }", result.IsError );
            }
            
            return ( tag.Id, result, time );
        }
        #endregion
        
        #region UpdateDate

        public void DisableProducts( DateTime dateTime )
        {
            MeasureWorkTime( () => DoDisableProducts( dateTime, null ) );
        }

        public void DisableProductsByShop( DateTime dateTime, string shopId )
        {
            MeasureWorkTime( () => DoDisableProducts( dateTime, shopId ) );
        }
        
        private void DoDisableProducts( DateTime dateTime, string shopId )
        {
            var result = Measure( () => _elasticClient.DisableOldProducts( dateTime, shopId ), out var time );
            if( result.IsError ) {
                _context.AddMessage( $"Disabled products {result.Pretty}", result.IsError );
            }
            LogWriter.Log( $"{result} товаров распродано", true );
        }

        #endregion

        #region Property

        public void LinkProperties(
            IEnumerable<BaseProperty> colors,
            IEnumerable<BaseProperty> materials,
            IEnumerable<BaseProperty> sizes )
        {

            _context.TotalActions = colors.Count() + materials.Count() + sizes.Count();
            
            ColorsLink( colors );
            MaterialsLink( materials );
            SizesLink( sizes );
        }

        public void UnlinkProperties(
            IEnumerable<BaseProperty> colors,
            IEnumerable<BaseProperty> materials,
            IEnumerable<BaseProperty> sizes )
        {
            _context.TotalActions = colors.Count() + materials.Count() + sizes.Count();
            
            ColorsUnlink( colors );
            MaterialsUnlink( materials );
            SizesUnlink( sizes );
        }
        
        
        public void ColorsUnlink( IEnumerable<BaseProperty> colors )
        {
            MeasureWorkTime( () => DoPropertyUnlink( colors, "color-unlink" ) );
        }

        public void MaterialsUnlink( IEnumerable<BaseProperty> materials )
        {
            MeasureWorkTime( () => DoPropertyUnlink( materials, "material-unlink" ) );
        }
        
        public void SizesUnlink( IEnumerable<BaseProperty> sizes )
        {
            MeasureWorkTime( () => DoPropertyUnlink( sizes, "size-unlink" ) );
        }
        
        private void DoPropertyUnlink( IEnumerable<BaseProperty> properties, string entity ) {
            var results = properties.Select( UnlinkProperty ).OrderByDescending( t => t.Item2.Updated );
            foreach( var (id, result, time) in results ) {
                Log( entity, id, result.Pretty, time.ToString() );
            }
        }
        
        private ( string, UpdateResult, long ) UnlinkProperty( BaseProperty property ) {
            var result = Measure( () => _elasticClient.UnlinkProductsByProperty( property ), out var time );
            _context.CalculatePercent();
            
            if( result.IsError ) {
                _context.AddMessage( $"id {property.Id} updated {result.Pretty}", result.IsError );
            }
            
            return ( property.Id, result, time );
        }
        
        public void ColorsLink( IEnumerable<BaseProperty> colors )
        {
            MeasureWorkTime( () => DoPropertyLink( colors, "color-link" ) );
        }

        public void MaterialsLink( IEnumerable<BaseProperty> materials )
        {
            MeasureWorkTime( () => DoPropertyLink( materials, "material-link" ) );
        }
        
        public void SizesLink( IEnumerable<BaseProperty> sizes )
        {
            MeasureWorkTime( () => DoPropertyLink( sizes, "size-link" ) );
        }
        
        private void DoPropertyLink( IEnumerable<BaseProperty> properties, string entity ) {
            var results = properties.Select( LinkProperty ).OrderByDescending( t => t.Item2.Updated );
            foreach( var (id, result, time) in results ) {
                Log( entity, id, result.Pretty, time.ToString() );
            }
        }
        
        private ( string, UpdateResult, long ) LinkProperty( BaseProperty property ) {
            var result = Measure( () => _elasticClient.LinkProductsByProperty( property ), out var time );
            _context.CalculatePercent();
            if( result.IsError ) {
                _context.AddMessage( $"id {property.Id} updated {result.Pretty}", result.IsError );
            }
            return ( property.Id, result, time );
        }
        
        #endregion
        
        private static void Log( string entity, string id, string count, string time ) =>
            LogWriter.Log( $"{entity}: {id}, count: { count }, time: {time}:" );

    }
}