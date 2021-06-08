// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class BrandshopWorker : BaseShopWorker, IShopWorker
    {
        
        public BrandshopWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }
        
        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Adult;
            
            
            return offer;
        }
    }
}