// a.snegovoy@gmail.com

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class DefaultShopWorker : BaseShopWorker, IShopWorker
    {
        public DefaultShopWorker(
            DbHelper dbHelper )
            : base( dbHelper ) { }
    }
}