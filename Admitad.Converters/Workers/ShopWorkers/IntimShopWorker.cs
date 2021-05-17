// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal class IntimShopWorker : BaseShopWorker, IShopWorker
    {
        protected override void FillParams(
            IExtendedOffer extendedOffer,
            RawOffer rawOffer )
        {
            extendedOffer.Age = Age.Adult;
            extendedOffer.Gender = Gender.Unisex;
            base.FillParams( extendedOffer, rawOffer );
        }
    }
}