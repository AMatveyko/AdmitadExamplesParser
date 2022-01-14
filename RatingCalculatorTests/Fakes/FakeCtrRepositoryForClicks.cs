using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculatorTests.Fakes
{
    internal class FakeCtrRepositoryForClicks : ICtrRepository
    {
        public List<ItemIds> GetCtrs() => new List<ItemIds>() {
            new ItemIds() {
                ProductId = "ec0aadc4ebf2c8bea920e4a91e4d930e",
                Views = 10000,
                Clicks = 2568
            }
        };
    }
}
