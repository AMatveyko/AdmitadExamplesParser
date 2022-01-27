using System.Collections.Generic;

using Common.Entities.Rating;


using TheStore.Api.Front.Data.Pub;

namespace RatingCalculatorTests.Fakes
{
    internal class FakeCtrRepositoryForClicks : ICtrRepository
    {
        public List<ItemCtrInfo> GetCtrs() => new () {
            new (1, "ec0aadc4ebf2c8bea920e4a91e4d930e", 10000, 2568)
        };
    }
}
