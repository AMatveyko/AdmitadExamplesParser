// a.snegovoy@gmail.com

using Admitad.Converters.Entities;
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
            // Handlers.Add( new AgeAndGenderFromCategoryShop( dbHelper ) );
            Handlers.Add( new ProcessPropertiesCategory( new VipAvenueCategoryContainer() ) );
        }
    }
}