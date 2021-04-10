// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadExamplesParser.Entities;

using AdmitadSqlData.Helpers;

namespace AdmitadExamplesParser.Workers.Components
{
    public class ProductLinker : BaseComponent
    {

        private static readonly Dictionary<string, List<int>> CategoryByProducts = new();
        private readonly IElasticClient<Product> _elasticClient;

        public ProductLinker( ElasticSearchClientSettings settings )
            : base( ComponentType.ProductLinker ) {
            _elasticClient = new ElasticSearchClient<Product>( settings );
        }




        #region Category
        public void CategoryLink()
        {
            MeasureWorkTime( DoCategoryLink );
        }

        private void DoCategoryLink()
        {
            var categories = DbHelper.GetCategories();
            var results = categories.Select( LinkCategory ).OrderByDescending( c => c.Item2 );
            foreach( var (id, count, time) in results ) {
                Log( "category", id, count, time.ToString() );
            }
        }

        private ( string, string, long ) LinkCategory( Category category )
        {
            var count = Measure( () => _elasticClient.UpdateProductsForCategoryFieldNameModel( category ), out var time );
            // var count = Measure( () => _elasticClient.UpdateProductsForCategory( category ), out var time );
            return ( category.Id, count, time );
        }
        #endregion
        
        #region Tag
        public void TagsLink() {
            MeasureWorkTime( DoTagsLink );
        }

        private void DoTagsLink() {
            var tags = DbHelper.GetTags();
            var results = tags.Select( LinkTag ).OrderByDescending( t => t.Item2 );
            foreach( var (id, count, time) in results ) {
                Log( "tag", id, count.ToString(), time.ToString() );
            }
        }
        
        private ( string, string, long ) LinkTag( Tag tag )
        {
            var count = Measure( () => _elasticClient.UpdateProductsForTag( tag ), out var time );
            return ( tag.Id, count, time );
        }
        #endregion
        
        #region UpdateDate

        public void DisableProducts( DateTime dateTime )
        {
            MeasureWorkTime( () => DoDisableProducts( dateTime ) );
        }

        private void DoDisableProducts( DateTime dateTime )
        {
            var counts = Measure( () => _elasticClient.DisableOldProducts( dateTime ), out var time );
            LogWriter.Log( $"Turned off products {counts}" );
        }
        
        #endregion

        #region Property

        public void LinkProperties()
        {
            ColorsLink();
            MaterialsLink();
            SizesLink();
        }

        public void UnlinkProperties()
        {
            ColorsUnlink();
            MaterialsUnlink();
            SizesUnlink();
        }
        
        
        public void ColorsUnlink()
        {
            MeasureWorkTime( () => DoPropertyUnlink( DbHelper.GetColors(), "color-unlink" ) );
        }

        public void MaterialsUnlink()
        {
            MeasureWorkTime( () => DoPropertyUnlink( DbHelper.GetMaterials(), "material-unlink" ) );
        }
        
        public void SizesUnlink()
        {
            MeasureWorkTime( () => DoPropertyUnlink( DbHelper.GetSizes(), "size-unlink" ) );
        }
        
        private void DoPropertyUnlink( IEnumerable<BaseProperty> properties, string entity ) {
            var results = properties.Select( UnlinkProperty ).OrderByDescending( t => t.Item2 );
            foreach( var (id, count, time) in results ) {
                Log( entity, id, count, time.ToString() );
            }
        }
        
        private ( string, string, long ) UnlinkProperty( BaseProperty property ) {
            var count = Measure( () => _elasticClient.UnlinkProductsByProperty( property ), out var time );
            return ( property.Id, count, time );
        }
        
        public void ColorsLink()
        {
            MeasureWorkTime( () => DoPropertyLink( DbHelper.GetColors(), "color-link" ) );
        }

        public void MaterialsLink()
        {
            MeasureWorkTime( () => DoPropertyLink( DbHelper.GetMaterials(), "material-link" ) );
        }
        
        public void SizesLink()
        {
            MeasureWorkTime( () => DoPropertyLink( DbHelper.GetSizes(), "size-link" ) );
        }
        
        private void DoPropertyLink( IEnumerable<BaseProperty> properties, string entity ) {
            var results = properties.Select( LinkProperty ).OrderByDescending( t => t.Item2 );
            foreach( var (id, count, time) in results ) {
                Log( entity, id, count, time.ToString() );
            }
        }
        
        private ( string, string, long ) LinkProperty( BaseProperty property ) {
            var count = Measure( () => _elasticClient.LinkProductsByProperty( property ), out var time );
            return ( property.Id, count, time );
        }
        
        #endregion
        
        private static void Log( string entity, string id, string count, string time ) =>
            LogWriter.Log( $"{entity}: {id}, count: { count }, time: {time}:" );

    }
}