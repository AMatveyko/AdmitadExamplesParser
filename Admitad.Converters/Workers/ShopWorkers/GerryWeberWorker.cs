// a.snegovoy@gmail.com

using System;

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GerryWeberWorker : BaseShopWorker, IShopWorker
    {
        public GerryWeberWorker(
            DbHelper dbHelper,
            Func<RawOffer, int, string> idGetter = null )
            : base( dbHelper, idGetter ) { Handlers.Add( new AlwaysAdultWomen() ); }
    }
}