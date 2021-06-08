// a.snegovoy@gmail.com

using System.Text.RegularExpressions;

using AdmitadSqlData.Helpers;

using Common.Entities;
using Common.Extensions;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GrettaWorker : BaseShopWorker, IShopWorker
    {

        private static Regex _countryPattern =
            new( "производства (- )?(?<countryName>([а-яА-Я]+))", RegexOptions.Compiled );
        
        public GrettaWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }
        
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