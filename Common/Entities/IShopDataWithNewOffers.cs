// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Common.Entities
{
    public interface IShopDataWithNewOffers
    {
        string Name { get; }
        List<RawOffer> NewOffers { get; }
    }
}