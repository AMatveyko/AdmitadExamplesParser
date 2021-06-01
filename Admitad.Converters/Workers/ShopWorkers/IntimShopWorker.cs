// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class IntimShopWorker : BaseShopWorker, IShopWorker
    {
        
        public IntimShopWorker( DbHelper dbHelper ) : base( dbHelper ) { }
        
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