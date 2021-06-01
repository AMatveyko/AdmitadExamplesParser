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
        
        public CategoryRepository( string connectionString, string version ) : base( connectionString, version ) { }
        
        public List<CategoryDb> GetCategoriesWithTerms() {
            using var db = GetDb();
            return db.Categories
                .Where( c => c.Enabled && c.Search != null && c.Search != string.Empty )
                // .OrderByDescending( c => c.Level )
                .ToList();
        }
        
        public List<CategoryDb> GetEnabledCategories() {
            using var db = GetDb();
            return db.Categories
                .Where( c => c.Enabled )
                // .OrderByDescending( c => c.Level )
                .ToList();
        }

        public List<CategoryDb> GetAllCategories() {
            using var db = GetDb();
            return db.Categories
                // .OrderByDescending( c => c.Level )
                .ToList();
        }

        public void ExcludeSearchField( string name )
        {
            var db = GetDb();
            var categories = db.Categories.ToList();
            foreach( var category in categories ) {
                var fields = category.Fields?.Split( ',' ).Where( f => f != name ).ToList();
                if( fields == null || fields.Any() == false ) {
                    continue;
                }
                
                category.Fields = string.Join( ',', fields );
            }

            db.SaveChanges();
        }
        

    }
}