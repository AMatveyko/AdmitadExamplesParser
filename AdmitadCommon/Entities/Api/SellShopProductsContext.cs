// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities.Api
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