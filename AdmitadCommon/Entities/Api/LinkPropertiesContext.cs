// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class LinkPropertiesContext : BackgroundBaseContext
    {
        public LinkPropertiesContext( string id )
            : base( GetCollectedId( nameof(LinkPropertiesContext), id ), id ) { }
    }
}