// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;
using Admitad.Converters.Helpers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class YoinsWorker : BaseShopWorker, IShopWorker
    {
        public YoinsWorker(
            DbHelper dbHelper )
            : base( dbHelper, ProductIdGetter.OfferIdAndShopId )
        {
            Handlers.Add( new AlwaysAdultWomen() );
        }
    }
}