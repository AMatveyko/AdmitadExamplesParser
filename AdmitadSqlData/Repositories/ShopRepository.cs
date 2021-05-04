// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;

using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal sealed class ShopRepository : BaseRepository
    {
        public List<XmlFileInfo> GetEnableShops()
        {
            using var db = GetDb();
            return db.Shops.Where( s => s.Enabled )
                .Select( s => new XmlFileInfo( s.Name, s.NameLatin, s.XmlFeed, s.Id ) ).ToList();
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

        public ShopRepository(
            string connectionString = null )
            : base( connectionString ) { }
    }
}