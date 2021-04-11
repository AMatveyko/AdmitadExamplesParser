// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace AdmitadExamplesParser.Workers.ShopWorkers
{
    internal interface IShopWorker
    {
        Offer Convert( RawOffer rawOfer );
    }
}