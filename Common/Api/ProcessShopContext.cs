// a.snegovoy@gmail.com

using Common.Entities;

namespace Common.Api
{
    public sealed class ProcessShopContext : BackgroundBaseContext
    {
        public ProcessShopContext( string id, int shopId, DownloadsInfo downloadsInfo, bool needSoldOut )
            : base( GetCollectedId( nameof(ProcessShopContext), id ), downloadsInfo.ShopName ) {
            ShopId = shopId;
            DownloadsInfo = downloadsInfo;
            NeedSoldOut = needSoldOut;
        }
        public int ShopId { get; }
        public bool NeedSoldOut { get; }
        public DownloadsInfo DownloadsInfo { get; }
        public int VersionProcessing => DownloadsInfo.VersionProcessing;
    }
}