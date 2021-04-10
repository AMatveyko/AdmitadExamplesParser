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
        
    }
}