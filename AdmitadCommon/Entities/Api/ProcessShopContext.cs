// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class ProcessShopContext : BackgroundBaseContext
    {
        public ProcessShopContext( string id, int shopId, DownloadInfo downloadInfo )
            : base( GetCollectedId( nameof(ProcessShopContext), id ), downloadInfo.ShopName ) {
            ShopId = shopId;
            DownloadInfo = downloadInfo;
        }
        public int ShopId { get; }
        public DownloadInfo DownloadInfo { get; }  
    }
}