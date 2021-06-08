// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace AdmitadCommon.Entities.Api
{
    public sealed class IndexShopContext : ParallelBackgroundContext
    {
        public IndexShopContext(
            int shopId,
            bool downloadFresh,
            bool needLink,
            bool needSoldOut )
            :base( $"{nameof(IndexShopContext)}:{shopId}", shopId.ToString() )
        {
            ShopId = shopId;
            DownloadFresh = downloadFresh;
            NeedLink = needLink;
            NeedSoldOut = needSoldOut;
        }
        
        public int ShopId { get; }
        public string ShopName { get; set; }
        [ JsonIgnore ]
        public bool DownloadFresh { get; }
        [ JsonIgnore ]
        public bool NeedLink { get; }
        [ JsonIgnore ]
        public bool NeedSoldOut { get; }
    }
}