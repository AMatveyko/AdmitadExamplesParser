// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class IncantoShopWorker : BaseShopWorker, IShopWorker
    {
        public IncantoShopWorker( DbHelper dbHelper ) : base( dbHelper )
        {
            Handlers.Add( new AlwaysAdultWomen() );
        }
    }
}