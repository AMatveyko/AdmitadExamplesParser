using Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class CategoryRepository : BaseRepository
    {
        public CategoryRepository(DbSettings settings) : base(settings) { }

        public void AddCategories( IEnumerable<CategoryDb> categories) {
            Db.Categories.AddRange(categories);
            Db.SaveChanges();
        }
    }
}
