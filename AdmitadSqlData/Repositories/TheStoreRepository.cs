// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;

using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal sealed class TheStoreRepository : BaseRepository
    {
        
        public TheStoreRepository( string connectionString, string version ) : base( connectionString, version ) { }
        
        #region Properties
        public List<ColorDb> GetColors()
        {
            using var db = GetDb();
            return db.Colors.ToList();
        }

        public List<SostavDb> GetMaterials()
        {
            using var db = GetDb();
            return db.Materials.ToList();
        }

        public List<SizeDb> GetSizes()
        {
            using var db = GetDb();
            return db.Sizes.ToList();
        }
        #endregion
        
        #region Brands
        public List<BrandDb> GetBrands()
        {
            using var db = GetDb();
            return db.Brands.Where( b => b.Duplicate == false ).ToList();
        }

        public void ClearUnknownBrands()
        {
            using var db = GetDb();
            db.UnknownBrands.RemoveRange( db.UnknownBrands );
            db.SaveChanges();
        }

        public void AddUnknownBrands(
            IEnumerable<UnknownBrands> brands )
        {
            using var db = GetDb();
            db.UnknownBrands.AttachRange( brands );
            db.SaveChanges();
        }
        #endregion
        
        #region Statistics

        public void WriteShopProcessingLog( ParseLog logEntry )
        {
            var db = GetDb();
            db.ParseLogs.Attach( logEntry );
            db.SaveChanges();
        }
        
        public List<OptionDb> GetSettingsOptions()
        {
            using var db = GetDb();
            return db.SettingsOptions.ToList();
        }

        public void UpdateProductsByCategory(
            Category category,
            int before,
            int after )
        {
            using var db = GetDb();
            int.TryParse( category.Id, out var id );
            var entity = db.ProductsByCategories.FirstOrDefault( p => p.CategoryId == id );
            if( entity == null ) {
                entity = new ProductsByCategory {
                    CategoryId = id,
                    CategoryName = category.NameH1
                };
                db.ProductsByCategories.Add( entity );
            }

            entity.ProductsBeforeLinking = before;
            entity.ProductsAfterLinking = after;
            entity.UpdateDate = DateTime.Now;

            db.SaveChanges();
        }

        public List<ShopStatisticsDb> GetShopStatistics()
        {
            var db = GetDb();
            return db.ShopStatistics
                .Where( s => s.UpdateDate >= DateTime.Now.AddDays( -10 ) && s.UpdateDate <= DateTime.Now )
                .ToList();
        }

        public ShopStatisticsDb GetShopStatistics( int shopId )
        {
            var db = GetDb();
            return db.ShopStatistics.FirstOrDefault( s => s.ShopId == shopId ) ?? new ShopStatisticsDb {
                ShopId = shopId
            };
        }

        public void InsertShopStatistics( ShopStatisticsDb productStatistics )
        {
            using var db = GetDb();
            db.ShopStatistics.Add( productStatistics );
            db.SaveChanges();
        }

        #endregion

        #region Countries

        public void SaveUnknownCountries( List<UnknownCountry> countries )
        {
            FlushCountries();
            AddCountries( countries );
        }

        private void AddCountries( List<UnknownCountry> countries )
        {
            using var db = GetDb();
            db.UnknownCountries.AddRange( countries );
            db.SaveChanges();
        }
        
        private void FlushCountries()
        {
            using var db = GetDb();
            db.UnknownCountries.RemoveRange( db.UnknownCountries.ToList() );
            db.SaveChanges();
        }
        
        #endregion
        
        #region Categories

        public void UpdateShopCategories( List<ShopCategoryDb> categories )
        {
            if( categories.Any() == false ) {
                return;
            }
            
            var shopId = categories.First().ShopId;
            var db = GetDb();
            var listFromDb = db.ShopCategories.Where( c => c.ShopId == shopId ).ToList();
            foreach( var category in categories ) {
                var fromDb = listFromDb.FirstOrDefault( c => c.CategoryId == category.CategoryId );
                if( fromDb == null ) {
                    db.ShopCategories.Add( category );
                    continue;
                }

                fromDb.Name = category.Name;
                fromDb.ParentId = category.ParentId;
                // fromDb.WomenProductsNumber = category.WomenProductsNumber;
                // fromDb.MenProductsNumber = category.MenProductsNumber;
                
                fromDb.UpdateDate = category.UpdateDate;
            }

            db.SaveChanges();
        }

        #endregion
    }
}