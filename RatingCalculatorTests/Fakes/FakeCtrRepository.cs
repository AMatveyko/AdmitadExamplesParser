using System.Collections.Generic;

using Common.Entities.Rating;

using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculatorTests.Fakes
{
    public sealed class FakeCtrRepository : ICtrRepository
    {
        public List<ItemCtrInfo> GetCtrs()
        {
            return new List<ItemCtrInfo>
            {
                new (1, "f8a04f25284dac6ce5681bfe5a673841", 101, 23),
                new (2, "948c96d4252bb92e68e115a7e2c9634e", 98, 10),
                new (3, "9b769cf7dfece9ddae39d8b08d04b18c", 1256, 245),
                new (4, "a2cd4609229a177a96289327ff99c486", 200, 0),
                new (5, "2704087abb1913c8adbf2e9a73a0c0a2", 197, 197),
                new (6, "2a4ba9378b7a86d307a41c87f7e7a86a", 97, 97)
            };
        }
    }
}
