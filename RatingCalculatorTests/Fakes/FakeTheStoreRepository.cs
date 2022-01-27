using System.Collections.Generic;

using Common.Entities.Rating;

using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculatorTests.Fakes
{
    internal sealed class FakeTheStoreRepository : ITheStoteRepositoryForRatingCalculator
    {
        public List<ItemCtrInfo> GetCtrs() => new FakeCtrRepository().GetCtrs();

        public List<ShopDb> GetShops() => new FakeShopRepository().GetShops();
    }
}
