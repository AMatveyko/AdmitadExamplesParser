// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class LinkPropertiesContext : BackgroundBaseContext
    {
        public LinkPropertiesContext( string id )
            : base( GetCollectedId( nameof(LinkPropertiesContext), id ), id ) { }
    }
}