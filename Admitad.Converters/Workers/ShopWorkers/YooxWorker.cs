// a.snegovoy@gmail.com

using System;
using System.Linq;

using AdmitadCommon.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    public class YooxWorker : BaseShopWorker, IShopWorker {
        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            var ageParam = rawOffer.Params.FirstOrDefault( p => p.Name == AgeParamName );
            var genderParam = rawOffer.Params.FirstOrDefault( p => p.Name == GenderParamName );
            if( ageParam != null ) {
                offer.Age = ageParam.Value switch {
                    "взрослый" => Age.Adult,
                    "none" => Age.All,
                    "дети" => Age.Child,
                    "младший" => Age.Child,
                    "ребенок" => Age.Child,
                    _ => Age.All
                };
            }

            if( genderParam != null ) {
                var gender = genderParam.Value switch {
                    "женщина" => Gender.Woman,
                    "мужчина" => Gender.Man,
                    "none" => Gender.Unisex,
                    _ => throw new ArgumentOutOfRangeException()
                };
                if( offer.Age == Age.Adult ) {
                    offer.Gender = gender;
                }

                if( offer.Age == Age.Child ) {
                    if( gender == Gender.Man ) {
                        offer.Gender = Gender.Boy;
                    }

                    if( gender == Gender.Woman ) {
                        offer.Gender = Gender.Girl;
                    }
                }
            }

            return offer;
        }
    }
}