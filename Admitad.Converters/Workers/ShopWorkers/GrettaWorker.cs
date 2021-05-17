// a.snegovoy@gmail.com

using System.Text.RegularExpressions;

using AdmitadCommon.Entities;
using AdmitadCommon.Extensions;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GrettaWorker : BaseShopWorker, IShopWorker
    {

        private static Regex _countryPattern =
            new( "производства (- )?(?<countryName>([а-яА-Я]+))", RegexOptions.Compiled );
        
        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            offer.Age = Age.Adult;
            if( offer.Model.IsNullOrWhiteSpace() ) {
                offer.Model = offer.CategoryPath;
            }

            if( offer.Description.IsNotNullOrWhiteSpace() ) {
                var m = _countryPattern.Match( offer.Description );
                if( m.Success ) {
                    offer.CountryId = DbHelper.GetCountryId( m.Groups["countryName"].Value );
                }
            }
            
            return offer;
        }
    }
}