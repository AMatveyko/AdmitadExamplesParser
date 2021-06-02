// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class IntimShopWorker : BaseShopWorker, IShopWorker
    {

        public IntimShopWorker( DbHelper dbHelper )
            : base( dbHelper )
        {
            Handlers.Add( new AlwaysAdultUndefined() );
        }
    }
}