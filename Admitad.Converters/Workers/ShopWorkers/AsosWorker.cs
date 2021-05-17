// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal class AsosWorker : BaseShopWorker, IShopWorker {
        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {

            var gender = rawOffer.Gender switch {
                "female" => Gender.Woman,
                "male" => Gender.Man,
                "unisex" => Gender.Unisex,
                _ => Gender.Undefined
            };

            if( gender != Gender.Undefined ) {
                offer.Gender = gender;
            }
            else if( offer.Gender == Gender.Undefined ) {
                offer.Gender = Gender.Unisex;
            }

            if( offer.Age == Age.Undefined ) {
                offer.Age = Age.Adult;
            }
            
            return offer;
        }
    }
}