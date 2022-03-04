// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal interface IShopWorker
    {
        Offer Convert( RawOffer rawOfer );
    }
}