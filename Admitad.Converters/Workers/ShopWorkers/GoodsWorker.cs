// a.snegovoy@gmail.com

using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GoodsWorker : BaseShopWorker, IShopWorker
    {

        private const string CountryParam = "Страна производства";
        
        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {

            offer.Age = Age.Adult;
            offer.Gender = Gender.Woman;
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