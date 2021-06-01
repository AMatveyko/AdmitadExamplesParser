// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public class RelinkCategoryContext : BackgroundBaseContext
    {
        public RelinkCategoryContext( string categoryId, bool relink )
            : base( GetCollectedId( nameof(RelinkCategoryContext), categoryId ), categoryId )
        {
            CategoryId = categoryId;
            Relink = relink;
        }
        public string CategoryId { get; }
        public string CategoryName { get; set; }
        public bool Relink { get; }
    }
}