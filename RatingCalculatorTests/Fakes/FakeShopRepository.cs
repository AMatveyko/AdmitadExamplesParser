using System.Collections.Generic;
using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Pub;

namespace RatingCalculatorTests.Fakes
{
    internal sealed class FakeShopRepository : IShopRepository
    {
        public List<ShopDb> GetShops() =>
            new List<ShopDb> {
                new ShopDb { Id = 1,  ECPC =  0.00 },
                new ShopDb { Id = 2,  ECPC =  0.10 },
                new ShopDb { Id = 3,  ECPC =  0.30 },
                new ShopDb { Id = 4,  ECPC =  0.44 },
                new ShopDb { Id = 5,  ECPC =  0.45 },
                new ShopDb { Id = 6,  ECPC =  1.00 },
                new ShopDb { Id = 7,  ECPC =  1.30 },
                new ShopDb { Id = 8,  ECPC =  2.20 },
                new ShopDb { Id = 9,  ECPC =  2.60 },
                new ShopDb { Id = 10, ECPC =  4.00 },
                new ShopDb { Id = 11, ECPC =  6.00 },
                new ShopDb { Id = 12, ECPC =  7.80 },
                new ShopDb { Id = 13, ECPC =  9.00 },
                new ShopDb { Id = 14, ECPC = 10.10 },
                new ShopDb { Id = 15, ECPC = 10.20 },
                new ShopDb { Id = 16, ECPC = 10.70, Enabled = true },
                new ShopDb { Id = 17, ECPC = 20.00 },
                new ShopDb { Id = 18, ECPC = 21.00 },
                new ShopDb { Id = 19, ECPC = 21.50 },
                new ShopDb { Id = 20, ECPC = 21.90 },
                new ShopDb { Id = 21, ECPC = 23.10 },
                new ShopDb { Id = 22, ECPC = 23.40 },
                new ShopDb { Id = 23, ECPC = 25.00 },
                new ShopDb { Id = 24, ECPC = 34.00 },
                new ShopDb { Id = 25, ECPC =  9.00 },
                new ShopDb { Id = 26, ECPC = 10.10 },
                new ShopDb { Id = 27, ECPC = 10.20 },
                new ShopDb { Id = 28, ECPC = 10.70 },
                new ShopDb { Id = 29, ECPC = 20.00 },
                new ShopDb { Id = 30, ECPC = 21.00 },
                new ShopDb { Id = 31, ECPC = 21.50 },
                new ShopDb { Id = 32, ECPC = 21.90 },
                new ShopDb { Id = 33, ECPC = 23.10 },
                new ShopDb { Id = 34, ECPC = 23.40 },
                new ShopDb { Id = 35, ECPC = 25.00 },
                new ShopDb { Id = 36, ECPC = 34.00 },
                new ShopDb { Id = 37, ECPC =  0.00 },
                new ShopDb { Id = 38, ECPC =  0.00, Weight = 10, Enabled = true },
                new ShopDb { Id = 39, ECPC =  0.00, Weight = 20, Enabled = false },
                new ShopDb { Id = 40, ECPC =  0.00 },
                new ShopDb { Id = 41, ECPC =  0.00 }
            };
    }
}
