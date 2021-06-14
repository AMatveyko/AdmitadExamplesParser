// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;
using Admitad.Converters.Helpers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class GoldenLineWorker : BaseShopWorker, IShopWorker
    {
        public GoldenLineWorker( DbHelper dbHelper )
            : base( dbHelper, ProductIdGetter.GroupIdAndShopId )
        {
            Handlers.Add( new OnlyAdult() );
        }
    }
}