// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AdmitadSqlData.Helpers;
using AdmitadSqlData.Repositories;

using Common.Entities;
using Common.Helpers;
using Common.Workers;

using NUnit.Framework;

namespace AdmitadSqlDataTests
{
    public sealed class TheStoreRepositoryTest
    {

        private static DbHelper GetDbHelper()
        {
            var settings = SettingsBuilder.GetDbSettings();
            return new DbHelper( settings );
        }
        
        [ Test ]
        public void GetChildren()
        {

            var dbHelper = GetDbHelper();
            
            List<Category> GetChildren(
                int categoryId ) {
                return dbHelper.GetCategoryChildren( categoryId );
            }

            var allCategory = dbHelper.GetAllCategories();
            
            var rootCategory = new List<int> {
                10000000,
                20000000,
                30000000,
                40000000,
                50000000,
                60000000,
                70000000
            };

            var ch = rootCategory.Select( GetChildren ).ToList();
            var sum = ch.Sum( l => l.Count );
            var notFound = allCategory.Where( cc => ch.SelectMany( c => c ).All( c => c.Id != cc.Id ) ).ToList();
        }
        
        [ Test, Explicit ]
        public void GetShopsTest()
        {
            var settings = SettingsBuilder.GetDbSettings();
            var rep = new ShopRepository( settings.GetConnectionString(), settings.Version );
            var shops = rep.GetEnableShops();
        }

        [ Test ]
        public void UpdateTag()
        {
            var helper = GetDbHelper();
            helper.UpdateTags();
        }

        [ Test ]
        public void SaveCountries()
        {
            var helper = GetDbHelper();
            helper.SaveUnknownCountries();
        }
        
        [ Test ]
        public void DeleteWordFromTag()
        {
            var helper = GetDbHelper();
            helper.DeleteWordFromTag( "блузки", 10103000 );
        }

        [ Test ]
        public void ExcludeSearchField()
        {
            var helper = GetDbHelper();
            helper.ExcludeSearchField( "categoryName" );
        }
        
        [ Test ]
        public void WarmCategories()
        {
            var helper = GetDbHelper();
            var cats = File.ReadLines( @"o:\admitad\теплыеКатегории.txt" )
                .Select( s => s.Split(":", StringSplitOptions.TrimEntries ) )
                .Select( s => ( s[0], s[1] ))
                .ToList();
            var categories = helper.GetCategories();
            foreach( var cat in cats ) {
                var categoryName = categories.FirstOrDefault( c => c.Id == cat.Item1 ).Name ?? "noname"; 
                Console.WriteLine( $"{categoryName} : {cat.Item2}" );
            }
        }

        [ Test ]
        public void UnknownBrands()
        {
            var helper = GetDbHelper();
            var brands = new[] {"noname", "sdfadf", "", " ", null, "USHATAVA", "Грандсток", "Adidas", "Грандсток", "Eger", "Eger" };
            foreach( var brand in brands ) {
                var cleanName = BrandHelper.GetClearlyVendor( brand );
                helper.RememberVendorIfUnknown( cleanName, brand );
            }
            helper.WriteUnknownBrands();
        }

        [ Test ]
        public void UnknownCountries()
        {
            var helper = GetDbHelper();
            var countries = new[] {"Китай", "Шмитай"};
            foreach( var country in countries ) {
                Console.WriteLine( helper.GetCountryId( country ) );
            }
            helper.SaveUnknownCountries();
        }
        
        [ Test ]
        public void GetCountryTest()
        {
            var helper = GetDbHelper();
            var id1 = helper.GetCountryId( "Мексика" );
            var id2 = helper.GetCountryId( "из Мексики" );
            var id3 = helper.GetCountryId( "mexico" );
            var id4 = helper.GetCountryId( "PRC" );
            var id5 = helper.GetCountryId( "ШвЕцИя" );
            Assert.AreEqual( id1, 11 );
            Assert.AreEqual( id2, 11 );
            Assert.AreEqual( id3, 11 );
            Assert.AreEqual( id4, 9 );
            Assert.AreEqual( id5, 23 );
        }

        [ Test, Explicit ]
        public void GetCategories()
        {
            var settings = SettingsBuilder.GetDbSettings();
            var rep = new CategoryRepository( settings.GetConnectionString(), settings.Version );
            var categories = rep.GetCategoriesWithTerms();
        }

        [ Test ]
        public void GetProperty()
        {
            var helper = GetDbHelper();
            var colors = helper.GetColors();
            var materials = helper.GetMaterials();
            var sizes = helper.GetSizes();
            
            Assert.IsNotEmpty( colors );
            Assert.IsNotEmpty( materials );
            Assert.IsNotEmpty( sizes );
            
        }
    }
}