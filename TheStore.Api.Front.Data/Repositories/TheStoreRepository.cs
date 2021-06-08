// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Common.Entities;
using Common.Settings;

using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class TheStoreRepository : BaseRepository
    {

        public TheStoreRepository( string connectionString, string version )
            : base( connectionString, version ) { }

        public List<ProxyDb> GetProxies()
        {
            using var db = DB;
            return db.Proxies.ToList();
        }
        
        public void UpdateCompareList( IEnumerable<CompareListingDb> compareList )
        {
            using var db = DB;
            var fromDb = db.CompareListings.ToList();
            foreach( var line in compareList ) {
                var lineDb = fromDb.FirstOrDefault( f => f.Url == line.Url );
                
                if( lineDb == null ) {
                    DB.CompareListings.Add( line );
                    continue;
                }

                lineDb.AddDate = line.AddDate;
                lineDb.Visits = line.Visits;
                lineDb.OldSiteProductCount = line.OldSiteProductCount;
                lineDb.OldSiteShopCount = line.OldSiteShopCount;
                lineDb.NewSiteProductCount = line.NewSiteProductCount;
                lineDb.NewSiteShopCount = line.NewSiteShopCount;

            }

            DB.SaveChanges();

        }
    }
}