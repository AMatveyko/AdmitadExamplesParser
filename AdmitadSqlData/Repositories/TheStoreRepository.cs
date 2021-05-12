// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;

using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal class TheStoreRepository : BaseRepository
    {
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
            return db.ShopStatistics.ToList();
        }

        public ShopStatisticsDb GetShopStatistics(
            int shopId )
        {
            var db = GetDb();
            return db.ShopStatistics.FirstOrDefault( s => s.ShopId == shopId ) ?? new ShopStatisticsDb {
                ShopId = shopId
            };
        }

        public void UpdateShopStatistics(
            ShopStatistics statistics,
            DateTime updateDate )
        {
            var db = GetDb();
            var statisticsDb = db.ShopStatistics.FirstOrDefault( s => s.ShopId == statistics.ShopId );
            if( statisticsDb == null ) {
                statisticsDb = new ShopStatisticsDb {
                    ShopId = statistics.ShopId
                };
                db.ShopStatistics.Add( statisticsDb );
            }

            if( statistics.Error != null &&
                statistics.Error != statisticsDb.Error )
                statisticsDb.Error = statistics.Error;

            if( statistics.SoldoutAfter != null &&
                statistics.SoldoutAfter != statisticsDb.SoldoutAfter )
                statisticsDb.SoldoutAfter = statistics.SoldoutAfter.Value;

            if( statistics.SoldoutBefore != null &&
                statistics.SoldoutBefore != statisticsDb.SoldoutBefore )
                statisticsDb.SoldoutBefore = statistics.SoldoutBefore.Value;

            if( statistics.TotalAfter != null &&
                statistics.TotalAfter != statisticsDb.TotalAfter )
                statisticsDb.TotalAfter = statistics.TotalAfter.Value;

            if( statistics.TotalBefore != null &&
                statistics.TotalBefore != statisticsDb.TotalBefore )
                statisticsDb.TotalBefore = statistics.TotalBefore.Value;

            statisticsDb.UpdateDate = updateDate;

            db.SaveChanges();
        }

        #endregion"
    }
}