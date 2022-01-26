// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.IO;
using System.Linq;

using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal sealed class TagRepository : BaseRepository {

        public TagRepository( string connectionString, string version ) : base( connectionString, version ) { }
        
        public List<TagDb> GetTags()
        {
            using var db = GetDb();
            return db.Tags.Where( t => t.Enabled && t.Name != null && t.Name != "" ).ToList();
        }

        public void SetProductCountForTag( string tagId, int count ) {
            var db = GetDb();
            var tag = db.Tags.First( t => t.Id.ToString() == tagId );
            tag.NumberProducts = count;
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

        public void UpdateTagFields( HashSet<int> idsSet )
        {
            var db = GetDb();
            var tags = db.Tags.Where( t => idsSet.Contains( t.Id ) ).ToList();
            var neededTags = tags.Where( t => t.SearchFields.Contains( "description" ) == false )
                .Where( nt => nt.SearchFields == "name,model,typeprefix,categoryName,param").ToList();
            foreach( var tagDb in neededTags ) {
                tagDb.SearchFields = "name,model,typeprefix,categoryName,param,description";
            }

            db.SaveChanges();

            var lines = neededTags.Select( t => $"{t.Id}:{t.NameTitle}" );

            File.WriteAllLines(@"g:\admitadFeedsTests\changedTags.txt", lines);
            

        }
    }
}