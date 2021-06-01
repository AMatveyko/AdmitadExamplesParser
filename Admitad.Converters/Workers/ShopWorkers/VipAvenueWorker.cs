// a.snegovoy@gmail.com

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class VipAvenueWorker : BaseShopWorker, IShopWorker
    {
        public VipAvenueWorker( DbHelper dbHelper ) : base( dbHelper ) { }
    }
}