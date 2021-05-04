// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace AdmitadCommon.Entities.Api
{
    public sealed class IndexShopContext : ParallelBackgroundContext
    {
        public IndexShopContext( int id, bool downloadFresh ) : base( $"{nameof(IndexShopContext)}:{id}", id.ToString() )
        {
            Id = id;
            DownloadFresh = downloadFresh;
        }
        
        public int Id { get; }
        public string Name { get; set; }
        [ JsonIgnore ]
        public bool DownloadFresh { get; }
    }
}