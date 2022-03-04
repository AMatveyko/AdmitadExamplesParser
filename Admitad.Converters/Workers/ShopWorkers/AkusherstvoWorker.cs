// a.snegovoy@gmail.com

using System;

using Admitad.Converters.Entities;
using Admitad.Converters.Handlers;
using Admitad.Converters.Helpers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class AkusherstvoWorker : BaseShopWorker, IShopWorker
    {
        public AkusherstvoWorker(
            DbHelper dbHelper,
            Func<RawOffer, int, string> idGetter = null,
            ProductType? type = null,
            AgeFromSize ageFromSize = null )
            : base( dbHelper, idGetter, type, ageFromSize ) {
            Handlers.Add( new ProcessPropertiesCategory( new AkusherstvoCategoryContainer() ) );
        }
    }
}