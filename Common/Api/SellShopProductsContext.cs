// a.snegovoy@gmail.com

namespace Common.Api
{
    public sealed class SellShopProductsContext : BackgroundBaseContext
    {
        public SellShopProductsContext( int id )
            : base( GetCollectedId( id.ToString(), nameof(SellShopProductsContext) ), nameof(SellShopProductsContext) )
        {
            ShopId = id;
        }
        
        public int ShopId { get; }
    }
}