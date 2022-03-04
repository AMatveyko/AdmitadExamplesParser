// a.snegovoy@gmail.com

using Admitad.Converters.Entities;
using Admitad.Converters.Handlers;
using Admitad.Converters.Helpers;
using AdmitadSqlData.Helpers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class LassieWorker : BaseShopWorker, IShopWorker
    {
        public LassieWorker(
            DbHelper dbHelper )
            : base( dbHelper, ProductIdGetter.FromName ) {
            Handlers.Add( new ProcessPropertiesCategory( new LassieCategoryContainer() ) );
        }
    }
}