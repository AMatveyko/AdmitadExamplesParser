// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;
using Admitad.Converters.Helpers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class BelleyouWorker : BaseShopWorker, IShopWorker
    {
        public BelleyouWorker( DbHelper dbHelper )
            : base( dbHelper, ProductIdGetter.OfferIdAndShopId )
        {
            Handlers.Add( new AlwaysAdultWomen() );
        }
    }
}