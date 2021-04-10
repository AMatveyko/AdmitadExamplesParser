// a.snegovoy@gmail.com

using AdmitadSqlData.Helpers;
using AdmitadSqlData.Repositories;

using NUnit.Framework;

namespace AdmitadSqlDataTests
{
    public class ShopRepositoryTest
    {
        [ Test, Explicit ]
        public void GetShopsTest()
        {
            var rep = new ShopRepository();
            var shops = rep.GetEnableShops();
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
            var categories = rep.GetCategories();
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