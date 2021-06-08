// a.snegovoy@gmail.com

using Common.Entities;

namespace Common.Api
{
    public sealed class ProcessShopContext : BackgroundBaseContext
    {
        public ProcessShopContext( string id, int shopId, DownloadInfo downloadInfo, bool needSoldOut )
            : base( GetCollectedId( nameof(ProcessShopContext), id ), downloadInfo.ShopName ) {
            ShopId = shopId;
            DownloadInfo = downloadInfo;
            NeedSoldOut = needSoldOut;
        }
        public int ShopId { get; }
        public bool NeedSoldOut { get; }
        public DownloadInfo DownloadInfo { get; }  
    }
}