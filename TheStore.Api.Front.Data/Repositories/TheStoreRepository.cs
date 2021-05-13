// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using TheStore.Api.Front.Data.DbContexts;
using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class TheStoreRepository
    {

        private readonly TheStoreDbContext _db;
        
        public TheStoreRepository( string connectionString, string version )
        {
            _db = new TheStoreDbContext( connectionString, version );
        }
        
        public void UpdateCompareList( IEnumerable<CompareListingDb> compareList )
        {

            var listFromDb =
                _db.CompareListings.ToDictionary( l => l.Url, l => l );
            
            foreach( var compare in compareList ) {

                if( listFromDb.ContainsKey( compare.Url ) == false ) {
                    _db.CompareListings.Add( compare );
                    return;
                }

                var compareDb = listFromDb[ compare.Url ];
                
                if( compareDb == null ) {
                    compareDb = new CompareListingDb { Url = compare.Url, AddDate = DateTime.Now };
                    _db.CompareListings.Add( compareDb );
                }

                if( compareDb.Visits != compare.Visits ) {
                    compareDb.Visits = compare.Visits;
                }

                if( compareDb.NewSiteProductCount != compare.NewSiteProductCount ) {
                    compareDb.NewSiteProductCount = compare.NewSiteProductCount;
                }

                if( compareDb.NewSiteShopCount != compare.NewSiteShopCount ) {
                    compareDb.NewSiteShopCount = compare.NewSiteShopCount;
                }

                if( compareDb.OldSiteProductCount != compare.OldSiteProductCount ) {
                    compareDb.OldSiteProductCount = compare.OldSiteProductCount;
                }

                if( compareDb.OldSiteShopCount != compare.OldSiteShopCount ) {
                    compareDb.OldSiteShopCount = compare.OldSiteShopCount;
                }

                compareDb.AddDate = compare.AddDate;

            }

            _db.SaveChanges();

        }
    }
}