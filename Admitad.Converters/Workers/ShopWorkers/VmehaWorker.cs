// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal class VmehaWorker : BaseShopWorker, IShopWorker
    {
        
        public VmehaWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }
        
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