// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class LinkTagsContext : BackgroundBaseContext
    {
        public LinkTagsContext( string id )
            : base( GetCollectedId( nameof(LinkTagsContext), id ), id ) {}
    }
}