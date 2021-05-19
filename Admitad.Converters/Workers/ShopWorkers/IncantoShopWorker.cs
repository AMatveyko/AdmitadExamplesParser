// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class IncantoShopWorker : BaseShopWorker, IShopWorker
    {
        public IncantoShopWorker()
        {
            _handlers.Add( new AlwaysAdultWomen() );
        }
    }
}