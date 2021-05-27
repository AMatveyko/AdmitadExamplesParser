// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class BelleyouWorker : BaseShopWorker, IShopWorker
    {
        public BelleyouWorker()
        {
            _handlers.Add( new AlwaysAdultWomen() );
        }
    }
}