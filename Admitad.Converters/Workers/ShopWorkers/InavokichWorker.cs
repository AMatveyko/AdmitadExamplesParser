// a.snegovoy@gmail.com

using System;
using System.Web;

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class InavokichWorker : BaseShopWorker, IShopWorker
    {
        public InavokichWorker(
            DbHelper dbHelper,
            Func<RawOffer, int, string> idGetter = null )
            : base( dbHelper, idGetter )
        {
            Handlers.Add( new OnlyWoman() );
        }

        protected override Offer GetTunedOffer(
            Offer offer,
            RawOffer rawOffer )
        {
            var url = HttpUtility.UrlDecode( offer.Url );
            
            offer.Age = Age.Adult;
            
            if( url.Contains( "/detskie_kostyumy/" ) ) {
                offer.Age = Age.Child;
            }

            return offer;
        }
    }
}