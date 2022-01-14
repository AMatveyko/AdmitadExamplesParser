using AdmitadSqlData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitadSqlData.Repositories
{
    internal sealed class CategoryMappingRepository : BaseRepository
    {
        public CategoryMappingRepository(string connectionString, string version) : base(connectionString, version) { }

        public List<CategoryMappingDb> GetCategoryMapping(int shopId) => GetDb().CategoryMapping.Where(c => c.ShopId == shopId).ToList();
    }
}
