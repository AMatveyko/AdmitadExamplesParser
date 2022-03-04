// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Common.Settings;

using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class TagsRepository : BaseRepository
    {
        public TagsRepository( DbSettings settings )
            : base( settings ) { }

        public List<TagDb> GetTagsByCategory( int categoryId ) =>
            Db.Tags.Where( t => t.CategoryId == categoryId ).ToList();

        public void AddNewTags(
            IEnumerable<TagDb> newTags )
        {
            Db.Tags.AddRange( newTags );
            Db.SaveChanges();
        }
    }
}