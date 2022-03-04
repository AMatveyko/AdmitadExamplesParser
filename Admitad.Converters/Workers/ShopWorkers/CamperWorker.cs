// a.snegovoy@gmail.com

using System;
using System.Web;

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class CamperWorker : BaseShopWorker, IShopWorker
    {
        public CamperWorker(
            DbHelper dbHelper,
            Func<RawOffer, int, string> idGetter = null )
            : base( dbHelper, idGetter ) { Handlers.Add( new NameFromDescription() ); }

        protected override Offer GetTunedOffer( Offer offer, RawOffer rawOffer )
        {
            var url = HttpUtility.UrlDecode( offer.Url );
            
            offer.Age = Age.Adult;
            
            if( url.Contains( "/kids/" ) ) {
                offer.Age = Age.Child;
                if( offer.Description.Contains( "для мальчиков" ) ) {
                    offer.Gender = Gender.Man;
                }

                if( offer.Description.Contains( "для девочек" ) ) {
                    offer.Gender = Gender.Woman;
                }
            }

            if( url.Contains( "/women/" ) ) {
                offer.Gender = Gender.Woman;
            }

            if( url.Contains( "/men/" ) ) {
                offer.Gender = Gender.Man;
            }

            return offer;
        }
    }
}