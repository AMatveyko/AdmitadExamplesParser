// a.snegovoy@gmail.com

namespace TheStore.Api.Front.Entity
{
    internal sealed class UrlInfo
    {
        public UrlInfo( int visits, string oldSiteUrl, string newSiteUrl ) =>
            ( Visits, OldSiteUrl, NewSiteUrl ) = ( visits, oldSiteUrl, newSiteUrl );
        public int Visits { get; }
        public string OldSiteUrl { get; }
        public string NewSiteUrl { get; } 
    }
}