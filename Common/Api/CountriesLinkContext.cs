// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class CountriesLinkContext : BackgroundBaseContext
    {
        public CountriesLinkContext( string id )
            : base( GetCollectedId( nameof( CountriesLinkContext ), id ), id ) { }
    }
}