// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Common.Settings;

using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class TagsWorkRepository : BaseRepository
    {
        internal TagsWorkRepository( DbSettings settings )
            : base( settings ) { }


        public List<OtherTagDb> GetOtherTags( string titleCondition1, string titleCondition2 ) =>
            Db.OtherTags.Where( t =>
                    t.Title.ToLower().Contains( titleCondition1 )
                    && t.Title.ToLower().Contains( titleCondition2 ) )
                .ToList();

        public void AddNewTags( IEnumerable<TagDb> tags ) {
            Db.Tags.AddRange( tags );
            Db.SaveChanges();
        }

        public List<CategoryDb> GetCategories( string likeName ) =>
            Db.Categories.Where( c => c.Name.ToLower().Contains( likeName ) ).ToList();

    }
}