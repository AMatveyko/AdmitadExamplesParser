// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class BrandshopWorker : BaseShopWorker, IShopWorker
    {
        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Adult;
            
            
            return offer;
        }
    }
}