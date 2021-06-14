// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Admitad.Converters.Workers.ShopWorkers;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;

namespace Admitad.Converters.Workers
{
    public sealed class OfferConverter : BaseComponent
    {
        private readonly IShopWorker _worker;
        private readonly List<RawOffer> _offers;

        public OfferConverter( IShopDataWithNewOffers shopData, DbHelper dbHelper, BackgroundBaseContext context  )
            : base( ComponentType.Converter, context )
        {
            _worker = ConverterBuilder.GetConverterByShop( shopData.Name, dbHelper );
            _offers = shopData.NewOffers;
        }

        public List<Offer> GetCleanOffers() =>
            MeasureWorkTime( DoGetCleanOffers );

        private List<Offer> DoGetCleanOffers()
        {
            var offers = FilterOffers( _offers ).Select( _worker.Convert ).ToList();
            _context.AddMessage( "Offers clean" );
            return offers;
        }

        private static IEnumerable<RawOffer> FilterOffers( IEnumerable<RawOffer> offers ) =>
            offers.Where( o => o != null )
                .Where( o => o.Pictures != null )
                .Where( o => o.Pictures.Any() );
    }
}