// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Admitad.Converters.Workers.ShopWorkers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

namespace Admitad.Converters.Workers
{
    public sealed class OfferConverter : BaseComponent
    {
        private readonly IShopWorker _worker;
        private readonly List<RawOffer> _offers;

        public OfferConverter( ShopData shopData, BackgroundBaseContext context  )
            : base( ComponentType.Converter, context )
        {
            _worker = ConverterBuilder.GetConverterByShop( shopData.Name );
            _offers = shopData.Offers;
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