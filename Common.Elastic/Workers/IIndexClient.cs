// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Entities;

namespace Common.Elastic.Workers
{
    public interface IIndexClient : IClientForShopStatistics
    {
        List<ProductPart> SearchProductsByOffersIds( string[] ids );

        void UpsertProducts< T >(
            IReadOnlyList<T> products )
            where T : class, IIndexedEntities;

        void UpdateProductsAfterDeletingOffers< T >(
            IReadOnlyList<T> products )
            where T : ProductPart;

        List<ProductPart> GetProductsByIds( string[] ids );

        ProductPart GetFirstEnableProductByShopIdAndCategoryId( string shopId, string categoryId );

        Task<IProductPhotos> GetProductsPhotoAsync( string id, string index = null );
        List<ProductRatingInfoFromElastic> GetProductsForUpdatingRating();
        void UpdateProductRating(List<Product> products);
    }
}