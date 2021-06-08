// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class TamarisWorker : BaseShopWorker, IShopWorker
    {
        public TamarisWorker( DbHelper dbHelper ) : base( dbHelper )
        {
            Handlers.Add( new AlwaysAdultWomen() );
            Handlers.Add( new CategoryPathToModel() );
        }
    }
}