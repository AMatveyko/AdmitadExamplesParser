// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class ProcessShopContext : BackgroundBaseContext
    {
        public ProcessShopContext( string id, int shopId, string filePath, string shopName )
            : base( GetCollectedId( nameof(ProcessShopContext), id ), shopName ) {
            FilePath = filePath;
            ShopName = shopName;
            ShopId = shopId;
        }
        public int ShopId { get; }
        public string FilePath { get; }
        public string ShopName { get; }
    }
}