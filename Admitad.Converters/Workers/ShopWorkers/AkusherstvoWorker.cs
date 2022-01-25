// a.snegovoy@gmail.com

using System;

using Admitad.Converters.Helpers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class AkusherstvoWorker : BaseShopWorker, IShopWorker
    {
        public AkusherstvoWorker( DbHelper dbHelper, Func<RawOffer, int, string> idGetter = null, ProductType? type = null,
            AgeFromSize ageFromSize = null )
            : base( dbHelper, idGetter, type, ageFromSize ) { }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer ) {
            if( offer.Gender == Gender.Man ) {
                offer.Age = Age.Child;
            }

            return offer;
        }
    }
}