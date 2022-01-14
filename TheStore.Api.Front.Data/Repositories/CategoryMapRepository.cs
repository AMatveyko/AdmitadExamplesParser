using Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class CategoryMapRepository : BaseRepository
    {
        public CategoryMapRepository(DbSettings settings) : base(settings) { }

        public List<CategoryMapDb> GetByShopId(int shopId) =>
            Db.CategoryMaps.Where(cp => cp.ShopId == shopId).ToList();

        public void Write(IEnumerable<CategoryMapDb> maps)
        {
            Db.CategoryMaps.AddRange(maps);
            Db.SaveChanges();
        }

        public void Update(IEnumerable<CategoryMapDb> maps)
        {
            Db.CategoryMaps.AttachRange(maps);
            Db.SaveChanges();
        }
    }
}
