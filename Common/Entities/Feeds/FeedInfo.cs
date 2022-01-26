// a.snegovoy@gmail.com

namespace Common.Entities
{
    public sealed class FeedInfo : IFileInfo
    {

        public FeedInfo( string id, string url ) => ( Id, Url ) = ( id, url );
        
        public string Id { get; }
        public string Url { get; }
        public string FilePath { get; set; }
        public DownloadError Error { get; set; }
        public long FileSize { get; set; }
        public long DownloadTime { get; set; }
        public bool NeedDownload =>
            Error != DownloadError.Ok && Error != DownloadError.ClosedStore;
    }
}