// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace AdmitadCommon.Entities.Api
{
    public sealed class IndexShopContext : ParallelBackgroundContext
    {
        public IndexShopContext( int shopId, bool downloadFresh ) : base( $"{nameof(IndexShopContext)}:{shopId}", shopId.ToString() )
        {
            ShopId = shopId;
            DownloadFresh = downloadFresh;
        }
        
        public int ShopId { get; }
        public string ShopName { get; set; }
        [ JsonIgnore ]
        public bool DownloadFresh { get; }
    }
}