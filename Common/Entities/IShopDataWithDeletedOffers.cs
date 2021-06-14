// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Common.Entities
{
    public interface IShopDataWithDeletedOffers
    {
        public List<RawOffer> DeletedOffers { get; }
    }
}