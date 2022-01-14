// a.snegovoy@gmail.com

using System;
using System.Linq;

using Admitad.Converters.Helpers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class BebaKidsWorker : BaseShopWorker, IShopWorker
    {

        private const string ForBaby = "для малышей";
        private const string ForChild = "детский";
        private const string ForAdult = "взрослый";
        
        private const string ForMan = "мужской";
        private const string ForWoman = "женский";
        private const string ForUnisex = "none";

        public BebaKidsWorker(
            DbHelper dbHelper,
            Func<RawOffer, int, string> idGetter = null,
            ProductType? type = null,
            AgeFromSize ageFromSize = null )
            : base( dbHelper, idGetter, type, ageFromSize ) { }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {
            FillAge( offer );
            FillGender( offer, GenderParamName );
            return offer;
        }

        private static void FillGender( Offer offer, string[] genderParamName )
        {
            var value = offer.Params.FirstOrDefault( p => genderParamName.Contains( p.Name ) )?
                .Values.Distinct().FirstOrDefault();
            offer.Gender = value switch {
                ForMan => Gender.Man,
                ForWoman => Gender.Woman,
                ForUnisex => Gender.Unisex,
                _ => offer.Gender
            };
        }
        
        private static void FillAge( Offer offer )
        {
            var value = offer.Params.FirstOrDefault( p => p.Name == AgeParamName )?.Values
                .Distinct().FirstOrDefault();
            if( value == null ) {
                return;
            }

            offer.AgeRange = value switch {
                ForBaby => new AgeRange( to: 35 ),
                ForChild => new AgeRange( 36, 168 ),
                _ => null
            };

            offer.Age = value switch {
                ForAdult => Age.Adult,
                ForBaby => Age.Child,
                ForChild => Age.Child,
                _ => offer.Age
            };

        }
    }
}