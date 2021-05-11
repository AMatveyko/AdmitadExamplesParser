// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public class RelinkCategoryContext : BackgroundBaseContext
    {
        public RelinkCategoryContext(
            string categoryId ) : base( GetCollectedId( nameof(RelinkCategoryContext), categoryId ), categoryId )
        {
            CategoryId = categoryId;
        }
        public string CategoryId { get; }
        public string CategoryName { get; set; }
    }
}