// a.snegovoy@gmail.com

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
        
    }
}