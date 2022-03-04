// a.snegovoy@gmail.com

using System;

using Admitad.Converters.Entities;
using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

using Common.Entities;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class FarfetchWorker : BaseShopWorker, IShopWorker
    {
        public FarfetchWorker(
            DbHelper dbHelper,
            Func<RawOffer, int, string> idGetter = null,
            ProductType? type = null )
            : base( dbHelper, idGetter, type )
        {
            Handlers.Add( new ProcessPropertiesCategory( new FarfetchCategoryProperties() ) );
        }
    }
}