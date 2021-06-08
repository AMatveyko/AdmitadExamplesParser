// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class SvMoscowWorker : BaseShopWorker, IShopWorker
    {
        public SvMoscowWorker( DbHelper dbHelper ) : base( dbHelper ) { }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {
            offer.Age = Age.Adult;
            var genderParam = GetParamValueByName( rawOffer.Params, GenderParamName )?.ToLower().Trim();
            offer.Gender = genderParam switch {
                "м" => Gender.Man,
                "ж" => Gender.Woman,
                _ => Gender.Unisex
            };

            return offer;
        }
    }
}