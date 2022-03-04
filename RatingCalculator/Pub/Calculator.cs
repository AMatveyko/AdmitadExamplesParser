using Common.Api;
using Common.Entities.Rating;
using RatingCalculator.Common;
using RatingCalculator.Workers;
using System.Collections.Generic;
using System.Linq;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculator.Pub
{
    public sealed class Calculator : ICalculator
    {

        private readonly ICtrHelper _ctrHelper;
        private readonly ShopRatingHelper _shopsHelper;

        public Calculator(ITheStoteRepositoryForRatingCalculator dbRepository, CtrHelpersBuilder ctrHelpersBuilder) {
            _ctrHelper = ctrHelpersBuilder.Get(dbRepository);
            _shopsHelper = new ShopRatingHelper(dbRepository);
        }

        public List<ProductRating> GetRatings(List<ProductRatingData> infos) => infos.Select(GetRating).ToList();

        public ProductRating GetRating(ProductRatingData info) {

            var data = CreateData(info);
            return RatingConverter.Convert(data);
        }

        private Common.ProductRatingWorkData CreateData(ProductRatingData info) =>
            new(info.ProductId, info.SoldOut == false)
            {
                CTR = _ctrHelper.GetCtr(info.ProductId),
                ECPCRating = _shopsHelper.GetEcpc(info.ShopId),
                ShopInternalRating = _shopsHelper.GetInternalRating(info.ShopId),
                IsOpenProgramm = _shopsHelper.IsOpeningProgramm(info.ShopId)
            };

        public void ChangeContext(BackgroundBaseContext context) => _ctrHelper.ChangeContext(context);
    }
}
