// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class VipAvenueWorker : BaseShopWorker, IShopWorker
    {
        public VipAvenueWorker(
            DbHelper dbHelper )
            : base( dbHelper )
        {
            Handlers.Add( new AgeAndGenderFromCategoryShop( dbHelper ) );
        }
    }
}