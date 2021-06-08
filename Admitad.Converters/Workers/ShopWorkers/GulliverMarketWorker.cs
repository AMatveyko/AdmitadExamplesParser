// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GulliverMarketWorker : BaseShopWorker, IShopWorker
    {
        
        public GulliverMarketWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }
        
        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Child;
            return offer;
        }
    }
}