// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class UnlinkPropertiesContext : BackgroundBaseContext
    {
        public UnlinkPropertiesContext(
            string id )
            : base( GetCollectedId( nameof(UnlinkPropertiesContext), id ), id ) { }
    }
}