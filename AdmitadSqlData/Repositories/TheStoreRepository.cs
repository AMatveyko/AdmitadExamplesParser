// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal class TheStoreRepository : BaseRepository {

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

        public void AddUnknownBrands( IEnumerable<UnknownBrands> brands )
        {
            using var db = GetDb();
            db.UnknownBrands.AttachRange( brands );
            db.SaveChanges();
        }

        public List<OptionDb> GetSettingsOptions()
        {
            using var db = GetDb();
            return db.SettingsOptions.ToList();
        }

    }
}