// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Extensions;
using Common.Helpers;
using Common.Settings;
using Common.Workers;

using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public class ProductLinkTests
    {

        private static DbHelper _dbHelper = new DbHelper( SettingsBuilder.GetDbSettings() );
        
        private readonly ElasticSearchClientSettings _settings = new ElasticSearchClientSettings {
            // ElasticSearchUrl = "http://127.0.0.1:9200",
            ElasticSearchUrl = "http://185.221.152.127:9200",
            //ElasticSearchUrl = "http://127.0.0.1:8888",
            DefaultIndex = "products-1",
            FrameSize = 10000
        };

        [ Test ]
        public void DisableShopProducts()
        {
            var client = CreateClient();
            var result = client.DisableShopProducts( "160" );
        }
        
        [ Test ]
        public void DisableOldProducts()
        {
            var client = CreateClient();
            client.DisableOldProducts( DateTime.Now, "163" );
        }

        [ Test ]
        public void CountDisabledProducts()
        {
            var count = CreateClient().CountForDisableProducts( DateTime.Now, null );
        }

        [ Test ]
        public void LinkCountries()
        {
            var linker = new ProductLinker( _settings, _dbHelper, new CountriesLinkContext( "1" ) );

            var countries = _dbHelper.GetCountries();
            linker.LinkCountries( countries );
        }

        [ Test ]
        public void LinkCountry()
        {
            var country = _dbHelper.GetCountries().FirstOrDefault( c => c.Id == 19 );
            var client = CreateClient();
            var result = client.UpdateProductsFroCountry( country );
            ;
        }
        
        [ Test ]
        public void LinkProperties()
        {
            var linker = new ProductLinker( _settings, _dbHelper, new BackgroundBaseContext( "1", "name" ) );

            var colors = _dbHelper.GetColors();
            var materials = _dbHelper.GetMaterials();
            var sizes = _dbHelper.GetSizes();

            linker.ColorsLink( colors );
            linker.MaterialsLink( materials );
            linker.SizesLink( sizes );

            //LogWriter.WriteLog(  );
        }

        [ Test ]
        public void CategoriesLinkTest()
        {
            var linker = new ProductLinker( _settings, _dbHelper, new BackgroundBaseContext( "1", "name" ) );
            linker.LinkCategories( _dbHelper.GetCategories() );
        }

        [ Test ]
        public void UnlinkShop()
        {
            var client = CreateClient();
            var result = client.UnlinkShop( "146" );
        }
        
        [ Test ]
        public void CategoryRelinkTest()
        {
            var categoryId = "10107000";
            var category = _dbHelper.GetCategories().FirstOrDefault( c => c.Id == categoryId );
            var client = CreateClient( "products-1" );
            var unlinkResult = client.UnlinkCategory( category );
            var linkResult = client.UpdateProductsForCategoryFieldNameModel( category );
            Console.WriteLine( $"Unlinked: {unlinkResult.Updated}/{unlinkResult.Updated}, Linked: {linkResult}" );
        }

        [ Test ]
        public void RelinkCategory()
        {
            var category = _dbHelper.GetCategories().FirstOrDefault( c => c.Id == "20712020" ); 
            var linker = new ProductLinker( _settings, _dbHelper, new BackgroundBaseContext("1", "name") );
            linker.RelinkCategory( category );
        }

        [ Test ]
        public void CountryLinkTest()
        {
            var countries = _dbHelper.GetCountries().FirstOrDefault();
            var client = CreateClient( "products-1" );
            var result = client.UpdateProductsFroCountry( countries );
            Console.WriteLine( $"Linked: {result.Updated}/{result.Updated}");
        }
        
        [ Test ]
        public void TagRelinkTest()
        {
            TagRelinkTest( "4777" );
        }

        private void TagRelinkTest( string tagId )
        {
            var tag = _dbHelper.GetTags().FirstOrDefault( t => t.Id == tagId );
            var client = CreateClient( "products-1" );
            var unlinkResult = client.UnlinkTag( tag );
            var linkResult = client.UpdateProductsForTag( tag );
            Console.WriteLine( $"Unlinked: {unlinkResult.Updated}/{unlinkResult.Updated}, Linked: {linkResult}" );
        }

        [ Test ]
        public void TagLinkTest()
        {
            var tagId = "3";
            var client = CreateClient( "products-1" );
            var tag = _dbHelper.GetTags().FirstOrDefault( t => t.Id == tagId );
            var linkResult = client.UpdateProductsForTag( tag );
            Console.WriteLine( $"Linked: {linkResult}" );
        }

        [ Test ]
        public void UpdateAddDate()
        {
            var client = CreateClient();
            var result = client.UpdateAddDate();
        }
        
        [ Test ]
        public void RelinkTagsForCategory()
        {
            var categoryId = 41612013;
            var tagsForCategory = _dbHelper.GetTags().Where( t => t.IdCategory == categoryId ).ToList();
            foreach( var tag in tagsForCategory ) {
                TagRelinkTest( tag.Id );
            }
        }
        
        [ Test ]
        public void TagsLinkTest()
        {
            var linker = new ProductLinker( _settings, _dbHelper, new BackgroundBaseContext("1", "name") );
            linker.LinkTags( _dbHelper.GetTags() );
        }

        [ Test ]
        public void UnlinkedPropertyCounts()
        {
            var client = CreateClient();
            var colors = _dbHelper.GetColors();
            var materials = _dbHelper.GetMaterials();
            var sizes = _dbHelper.GetSizes();
            var allProperties = new List<( string, BaseProperty )>();
            allProperties.AddRange( colors.Select( i => ( i.FieldName, (BaseProperty) i ) ) );
            allProperties.AddRange( materials.Select( i => ( i.FieldName, (BaseProperty) i ) ) );
            allProperties.AddRange( sizes.Select( i => ( i.FieldName, (BaseProperty) i ) ) );
            var results =
                allProperties.Select( p => ( p.Item1, client.CountUnlinkProductsByProperty( p.Item2 ) ) )
                    .OrderByDescending( i => i.Item2 ).ToList();
        }

        [ Test ]
        public void UnlinkProperties()
        {
            var linker = new ProductLinker( _settings, _dbHelper, new BackgroundBaseContext("1", "name") );
            
            var colors = _dbHelper.GetColors();
            var materials = _dbHelper.GetMaterials();
            var sizes = _dbHelper.GetSizes();

            linker.UnlinkProperties( colors, materials, sizes );
        }
        
        [ Test ]
        public void ElasticCountApi()
        {
            var category = GetFirstCategory();
            var client = CreateClient();
            var count = client.CountProductsForCategory( category );
            Console.WriteLine( $"Category info" );
            Console.WriteLine( $"Searh { Join( category.Terms ) }" );
            Console.WriteLine( $"Exclude { Join( category.ExcludeTerms ) }" );
            Console.WriteLine( $"Fields { Join( category.Fields ) }" );
            Console.WriteLine( $"Suitable categories { count }" );
        }

        
        [ Test ]
        public void ProductsForTags()
        {
            var tags = GetTags();
            var client = CreateClient();
            var counts = tags.Select(
                    ( t, i ) => {
                        var result = ( t.Id, client.CountProductsForTag( t ) );
                        LogWriter.Log( i.ToString() );
                        return result;
                    } )
                .OrderByDescending( t => t.Item2 )
                .ToList();
            foreach( var (id, item2) in counts ) {
                LogWriter.Log( $"{id}: {item2}," );
            }
        }

        [ Test ]
        public void ProductsForCategories()
        {
            var categories = GetCategories();
            var client = CreateClient();
            var counts = categories.Select( c => ( c.Id, client.CountProductsForCategory( c ) ) )
                .OrderByDescending( c => c.Item2 )
                .ToList();
            foreach( var (id, item2) in counts ) {
                Console.Write( $"{id}: {item2}," );
            }
        }

        [ Test ]
        public void ProductWithCategory()
        {
            var client = CreateClient();
            var results = GetCategories().Select( c => ( c.Id, client.CountProductsWithCategory( c.Id ) ) )
                .OrderByDescending( c => c.Item2 );
            foreach( var ( id, count ) in results ) {
                Console.Write($"{id}: {count},");
            }
        }
        
        [ Test ]
        public void ProductWithTag()
        {
            var client = CreateClient();
            var results = GetTags().Select( t => ( t.Id, client.CountProductsWithTag( t.Id ) ) )
                .OrderByDescending( c => c.Item2 );
            foreach( var ( id, count ) in results ) {
                Console.Write($"{id}: {count},");
            }
        }
        
        [ Test ]
        public void ElasticUpdateApi()
        {
            var category = GetFirstCategory();
            category.Id = "10101000";
            var categoryId = category.Id;
            var client = CreateClient();

            var suitableCount = client.CountProductsForCategory( category );
            Console.WriteLine( $"Suitable products : { suitableCount }" );
            client.UpdateProductsForCategory( category );
            var count = client.CountProductsWithCategory( categoryId );
            Console.WriteLine( $"Products witch category id { categoryId } : { count }" );
        }
        
        private ElasticSearchClient<Product> CreateClient( string indexName = null )
        {
            if( indexName.IsNotNullOrWhiteSpace() ) {
                _settings.DefaultIndex = indexName;
            }
            return new ( _settings, new BackgroundBaseContext("1", "name") );
        }
        
        private static Category GetFirstCategory()
        {
            var categories = GetCategories();
            var category = categories.FirstOrDefault();
            if( category == null ) {
                Console.WriteLine( "Categories empty!" );
            }

            return category;
        }

        private static List<Category> GetCategories() => _dbHelper.GetCategories();
        private static List<Tag> GetTags() => _dbHelper.GetTags();
        
        private static string Join( string[] strings )
        {
            return string.Join( ",", strings );
        }
    }
}