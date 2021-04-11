// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace AdmitadExamplesParser.Workers.ShopWorkers
{
    internal interface IShopWorker
    {
        int CountWithOldPriceSecond { get; }
        Offer Convert( RawOffer rawOfer );
    }
}