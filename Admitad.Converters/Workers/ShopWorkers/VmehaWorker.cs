// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    public class VmehaWorker : BaseShopWorker, IShopWorker
    {
        protected override void FillParams(
            IExtendedOffer extendedOffer,
            RawOffer rawOffer )
        {
            extendedOffer.Age = Age.Adult;
            extendedOffer.Gender = Gender.Woman;
            base.FillParams( extendedOffer, rawOffer );
        }
    }
}