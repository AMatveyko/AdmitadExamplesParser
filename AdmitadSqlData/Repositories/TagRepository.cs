// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal class TagRepository : BaseRepository {

        public List<TagDb> GetTags()
        {
            using var db = GetDb();
            return db.Tags.Where( t => t.Enabled ).ToList();
        }

        public void AddDescriptionField()
        {
            const string description = nameof(description);
            using var db = GetDb();
            var tags = db.Tags.ToList();
            foreach( var tag in tags ) {
                if( tag.SearchFields.Contains( description ) ) {
                    continue;
                }

                tag.SearchFields = $"{tag.SearchFields},{description}";
            }

            db.SaveChanges();
        }

        public void DeleteWordFromTagSearch(
            string word,
            int categoryId )
        {
            var deleteAlways = new[] {"в ", "из ", "с "};
            using var db = GetDb();
            var tags = db.Tags.Where( t => t.IdCategory == categoryId ).ToList();
            foreach( var tag in tags ) {
                foreach( var alway in deleteAlways ) {
                    tag.Name = tag.Name.Replace( alway, string.Empty );
                }

                tag.Name = tag.Name.Replace( word, string.Empty ).Trim();
            }

            db.SaveChanges();
        }
        
    }
}