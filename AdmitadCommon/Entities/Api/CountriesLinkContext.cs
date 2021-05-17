// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class CountriesLinkContext : BackgroundBaseContext
    {
        public CountriesLinkContext( string id )
            : base( GetCollectedId( nameof( CountriesLinkContext ), id ), id ) { }
    }
}