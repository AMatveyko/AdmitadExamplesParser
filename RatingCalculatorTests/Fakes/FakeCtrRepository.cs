using System.Collections.Generic;
using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculatorTests.Fakes
{
    public sealed class FakeCtrRepository : ICtrRepository
    {
        public List<ItemIds> GetCtrs()
        {
            return new List<ItemIds>
            {
                new ItemIds {
                    ProductId = "f8a04f25284dac6ce5681bfe5a673841",
                    Views = 101,
                    Clicks = 23
                },
                new ItemIds {
                    ProductId = "948c96d4252bb92e68e115a7e2c9634e",
                    Views = 98,
                    Clicks = 10
                },
                new ItemIds {
                    ProductId = "9b769cf7dfece9ddae39d8b08d04b18c",
                    Views = 1256,
                    Clicks = 245
                },
                new ItemIds {
                    ProductId = "a2cd4609229a177a96289327ff99c486",
                    Views = 200,
                    Clicks = 0
                },
                new ItemIds
                {
                    ProductId = "2704087abb1913c8adbf2e9a73a0c0a2",
                    Views = 197,
                    Clicks = 197
                },
                new ItemIds
                {
                    ProductId = "2a4ba9378b7a86d307a41c87f7e7a86a",
                    Views = 97,
                    Clicks = 97
                }
            };
        }
    }
}
