// a.snegovoy@gmail.com

namespace TheStore.Api.Core.Sources.Entities
{
    public sealed class UrlInfo
    {
        public UrlInfo( int visits, string oldSiteUrl, string newSiteUrl ) =>
            ( Visits, OldSiteUrl, NewSiteUrl ) = ( visits, oldSiteUrl, newSiteUrl );
        public int Visits { get; }
        public string OldSiteUrl { get; }
        public string NewSiteUrl { get; } 
    }
}