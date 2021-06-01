// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class RelinkTagContext : BackgroundBaseContext
    {
        public RelinkTagContext( string tagId, bool relink )
            : base( GetCollectedId( nameof(RelinkTagContext), tagId ), tagId )
        {
            Relink = relink;
            TagId = tagId;
        }
        public string TagId { get; }
        public string Title { get; set; }
        public bool Relink { get; }
    }
}