// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class RelinkTagContext : BackgroundBaseContext
    {
        public RelinkTagContext(
            string tagId )
            : base( GetCollectedId( nameof(RelinkTagContext), tagId ), tagId )
        {
            TagId = tagId;
        }
        public string TagId { get; }
        public string Title { get; set; }
    }
}