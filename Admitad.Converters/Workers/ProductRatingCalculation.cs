using Admitad.Converters.Entities;
using Common.Api;
using Common.Entities;
using Common.Entities.Rating;
using Common.Helpers;
using RatingCalculator.Pub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Pub;

namespace Admitad.Converters.Workers
{
    public sealed class ProductRatingCalculation {

        private ICalculator _calculator;

        public ProductRatingCalculation(ITheStoteRepositoryForRatingCalculator repository, string ctrCalculationType ) {
            var calculationType = EnumHelper<CtrHelperType>.GetValueByName(ctrCalculationType);
            var builder = new CtrHelpersBuilder(calculationType );
            _calculator = new Calculator(repository, builder);
        }

        public void SetRating(Product product, bool soldOut) {
            var ratingInfo = GetRating(product.Id, product.ShopId, soldOut);
            SetRating(product, ratingInfo);
        }

        private ProductRating GetRating(string id, string shopId, bool soldOut) {
            var info = new ProductRatingData(id, shopId, soldOut);
            return _calculator.GetRating(info);
        }

        public List<Product> GetRatingUpdateDatas(List<ProductRatingInfoFromElastic> infos) =>
            infos.Select(info => (CurrentRating: info.Rating, ratingIfo: GetRating(info)))
            .Where(container => container.CurrentRating != container.ratingIfo.Rating)
            .Select(container => GetRatingUpdateData(container.ratingIfo)).ToList();


        public void ChangeContext(BackgroundBaseContext context) => _calculator.ChangeContext(context);

        private Product GetRatingUpdateData(ProductRating ratingInfo) {
            var data = new Product() { Id = ratingInfo.ProductId };
            SetRating(data, ratingInfo);
            return data;
        }

        private ProductRating GetRating(ProductRatingInfoFromElastic product) {
            var soldOut = product.Soldout == 1 ? true : false;
            return GetRating(product.Id, product.ShopId, soldOut);
        }

        private static void SetRating(IProductRatingUpdateData data, ProductRating ratingInfo) {
            data.Rating = ratingInfo.Rating;
            data.RatingUpdateDate = DateTime.Now;
        }

    }
}
