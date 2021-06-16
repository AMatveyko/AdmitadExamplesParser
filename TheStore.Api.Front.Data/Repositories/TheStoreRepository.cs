// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Common.Entities;
using Common.Settings;

using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class TheStoreRepository : BaseRepository, ISettingsRepository
    {

        private static readonly Dictionary<string, int> Cache = new Dictionary<string, int>();

        public TheStoreRepository( string connectionString, string version )
            : base( connectionString, version ) { }

        public List<ProxyDb> GetProxies()
        {
            using var db = Db;
            return db.Proxies.ToList();
        }

        public void UpdateCategoryAgeAndGender( List<(string, Age, Gender)> updateData, int shopId )
        {
            var categories = Db.ShopCategories.Where( c => c.ShopId == shopId ).ToList();
            foreach( var data in updateData ) {
                var category = categories.First( c => c.CategoryId == data.Item1 );
                category.AgeId = GetAgeId( data.Item2.ToString() );
                category.SexId = GetSexId( data.Item3.ToString() );
            }

            Db.SaveChanges();
        }
        
        public List<CategoryShopDb> GetShopCategories( int shopId )
        {
            return Db.ShopCategories.Where( c => c.ShopId == shopId ).ToList();
        }
        
        public ShopDb GetShopById( int id )
        {
            return Db.Shops.First( s => s.Id == id );
        }
        
        public void UpdateCompareList( IEnumerable<CompareListingDb> compareList )
        {
            var fromDb = Db.CompareListings.ToList();
            foreach( var line in compareList ) {
                var lineDb = fromDb.FirstOrDefault( f => f.Url == line.Url );
                
                if( lineDb == null ) {
                    Db.CompareListings.Add( line );
                    continue;
                }

                lineDb.AddDate = line.AddDate;
                lineDb.Visits = line.Visits;
                lineDb.OldSiteProductCount = line.OldSiteProductCount;
                lineDb.OldSiteShopCount = line.OldSiteShopCount;
                lineDb.NewSiteProductCount = line.NewSiteProductCount;
                lineDb.NewSiteShopCount = line.NewSiteShopCount;

            }

            Db.SaveChanges();

        }

        public List<SettingsOption> GetSettingsOptions() =>
            Db.CoreSettings.Select( cs => new SettingsOption { Option = cs.Option, Value = cs.Value } )
                .ToList();

        private int GetAgeId( string age ) =>
            GetFromCache(
                age,
                (
                    value ) => Db.Ages.ToList().First( a => a.Name == age ).Id );

        private int GetSexId( string gender ) =>
            GetFromCache(
                gender,
                (
                    value ) => Db.Sex.ToList().First( s => s.Name == value ).Id );

        private static int GetFromCache( string key, Func<string, int> valueGetter )
        {
            if( Cache.ContainsKey( key ) == false ) {
                Cache[ key ] = valueGetter( key );
            }

            return Cache[ key ];
        }
    }
}