// a.snegovoy@gmail.com

using System.Linq;

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

using Common.Entities;
using Common.Extensions;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GoodsWorker : BaseShopWorker, IShopWorker
    {

        private const string CountryParam = "Страна производства";

        public GoodsWorker(
            DbHelper dbHelper )
            : base( dbHelper )
        {
            Handlers.Add( new AlwaysAdultWomen() );
            Handlers.Add( new NameFromDescription());
        }
        
        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {
            var countryParam = offer.Params.FirstOrDefault( p => p.Name == CountryParam );
            if( countryParam != null && countryParam.Values.Any( v => v.IsNotNullOrWhiteSpace() ) ) {
                offer.CountryId = DbHelper.GetCountryId( countryParam.Values.FirstOrDefault( v => v.IsNotNullOrWhiteSpace() ) );
            }
            if( offer.Model.IsNullOrWhiteSpace() ) {
                offer.Model = offer.CategoryPath;
            }

            return offer;
        }
        
    }
}