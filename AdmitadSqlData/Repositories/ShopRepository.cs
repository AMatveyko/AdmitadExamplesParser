﻿// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;

using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal sealed class ShopRepository : BaseRepository
    {
        
        public ShopRepository( string connectionString, string version )
            : base( connectionString, version ) { }
        
        public List<Shop> GetEnableShops()
        {
            using var db = GetDb();
            return db.Shops.Where( s => s.Enabled ).ToList();
        }

        public Shop GetShop( int id )
        {
            using var db = GetDb();
            return db.Shops.First( s => s.Id == id );
        }

        public int GetShopId( string shopNameLatin )
        {
            using var db = GetDb();
            return db.Shops.First( s => s.NameLatin == shopNameLatin ).Id;
        }

        public void UpdateDate( int shopId, DateTime updateDate )
        {
            using var db = GetDb();
            var shop = db.Shops.FirstOrDefault( s => s.Id == shopId );
            if( shop == null ) {
                return;
            }

            shop.UpdateDate = updateDate;
            db.SaveChanges();
        }
        
    }
}