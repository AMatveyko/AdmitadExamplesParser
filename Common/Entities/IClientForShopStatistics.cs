// a.snegovoy@gmail.com

namespace Common.Entities
{
    public interface IClientForShopStatistics
    {
        long CountDisabledProductsByShop(
            string shopId );

        long CountProductsForShop(
            string shopId );
    }
}