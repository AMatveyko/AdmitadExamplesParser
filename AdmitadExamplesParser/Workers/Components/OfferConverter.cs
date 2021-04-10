// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadCommon.Entities;

using AdmitadExamplesParser.Entities;
using AdmitadExamplesParser.Workers.ShopWorkers;

namespace AdmitadExamplesParser.Workers.Components
{
    internal sealed class OfferConverter : BaseComponent
    {
        private readonly IShopWorker _worker;
        private readonly List<RawOffer> _offers;

        public OfferConverter( ShopData shopData )
            : base( ComponentType.Converter )
        {
            _worker = ConverterBuilder.GetConverterByShop( shopData.Name );
            _offers = shopData.Offers;
        }

        public List<Offer> GetCleanOffers() =>
            MeasureWorkTime( DoGetCleanOffers );

        private List<Offer> DoGetCleanOffers() =>
            FilterOffers( _offers ).Select( _worker.Convert ).ToList();

        private static IEnumerable<RawOffer> FilterOffers( IEnumerable<RawOffer> offers ) =>
            offers.Where( o => o != null )
                .Where( o => o.Pictures != null )
                .Where( o => o.Pictures.Any() );
    }
}