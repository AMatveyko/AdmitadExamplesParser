// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
{
    public sealed class UnlinkShopContext : BackgroundBaseContext
    {
        public UnlinkShopContext(
            int shopId )
            : base( GetCollectedId( shopId.ToString(), nameof(UnlinkShopContext) ), nameof(UnlinkShopContext) )
        {
            ShopId = shopId.ToString();
        }
        
        public string ShopId { get; }
        
    }
}