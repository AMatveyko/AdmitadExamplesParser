using System;
using System.Collections.Generic;
using System.Text;
using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.Pub
{
    public interface IShopRepository
    {
        List<ShopDb> GetShops();
    }
}
