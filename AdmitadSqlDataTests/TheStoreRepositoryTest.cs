// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Helpers;
using AdmitadSqlData.Repositories;

using NUnit.Framework;

namespace AdmitadSqlDataTests
{
    public sealed class TheStoreRepositoryTest
    {

        [ Test ]
        public void GetChildren()
        {
            List<Category> GetChildren(
                int categoryId ) {
                return DbHelper.GetCategoryChildren( categoryId );
            }

            var allCategory = DbHelper.GetAllCategories();
            
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
            var rep = new ShopRepository();
            var shops = rep.GetEnableShops();
        }

        [ Test ]
        public void UpdateTag()
        {
            DbHelper.UpdateTags();
        }

        [ Test ]
        public void DeleteWordFromTag()
        {
            DbHelper.DeleteWordFromTag( "блузки", 10103000 );
        }
        
        [ Test ]
        public void WarmCategories()
        {
            var cats = File.ReadLines( @"o:\admitad\теплыеКатегории.txt" )
                .Select( s => s.Split(":", StringSplitOptions.TrimEntries ) )
                .Select( s => ( s[0], s[1] ))
                .ToList();
            var categories = DbHelper.GetCategories();
            foreach( var cat in cats ) {
                var categoryName = categories.FirstOrDefault( c => c.Id == cat.Item1 ).Name ?? "noname"; 
                Console.WriteLine( $"{categoryName} : {cat.Item2}" );
            }
        }

        [ Test ]
        public void UnknownBrands()
        {
            var brands = new[] {"noname", "sdfadf", "", " ", null, "USHATAVA", "Грандсток", "Adidas", "Грандсток", "Eger", "Eger" };
            foreach( var brand in brands ) {
                var cleanName = BrandHelper.GetClearlyVendor( brand );
                DbHelper.RememberVendorIfUnknown( cleanName );
            }
            DbHelper.WriteUnknownBrands();
        }
        
        [ Test ]
        public void GetCountryTest()
        {
            var id1 = DbHelper.GetCountryId( "Мексика" );
            var id2 = DbHelper.GetCountryId( "из Мексики" );
            var id3 = DbHelper.GetCountryId( "mexico" );
            var id4 = DbHelper.GetCountryId( "PRC" );
            var id5 = DbHelper.GetCountryId( "ШвЕцИя" );
            Assert.AreEqual( id1, 11 );
            Assert.AreEqual( id2, 11 );
            Assert.AreEqual( id3, 11 );
            Assert.AreEqual( id4, 9 );
            Assert.AreEqual( id5, 23 );
        }

        [ Test, Explicit ]
        public void GetCategories()
        {
            var rep = new CategoryRepository();
            var categories = rep.GetCategoriesWithTerms();
        }

        [ Test ]
        public void GetProperty()
        {
            var colors = DbHelper.GetColors();
            var materials = DbHelper.GetMaterials();
            var sizes = DbHelper.GetSizes();
            
            Assert.IsNotEmpty( colors );
            Assert.IsNotEmpty( materials );
            Assert.IsNotEmpty( sizes );
            
        }
    }
}