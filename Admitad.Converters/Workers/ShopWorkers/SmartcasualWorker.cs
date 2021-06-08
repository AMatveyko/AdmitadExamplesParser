// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class SmartcasualWorker : BaseShopWorker, IShopWorker
    {
        public SmartcasualWorker( DbHelper dbHelper ) : base( dbHelper )
        {
            Handlers.Add( new SoldOutIfAvailableFalse() );
        }
    }
}