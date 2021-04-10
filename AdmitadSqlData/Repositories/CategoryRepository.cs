// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Extensions;

using AdmitadSqlData.DbContexts;
using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal sealed class CategoryRepository : BaseRepository
    {
        public List<CategoryDb> GetCategories() {
            using var db = GetDb();
            return db.Categories
                .Where( c => c.Enabled && c.Search != null && c.Search != string.Empty )
                // .OrderByDescending( c => c.Level )
                .ToList();
        }

        public CategoryRepository(
            string connectionString = null )
            : base( connectionString ) { }
    }
}