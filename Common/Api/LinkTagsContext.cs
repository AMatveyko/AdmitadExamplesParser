// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class LinkTagsContext : BackgroundBaseContext
    {
        public LinkTagsContext( string id )
            : base( GetCollectedId( nameof(LinkTagsContext), id ), id ) {}
    }
}