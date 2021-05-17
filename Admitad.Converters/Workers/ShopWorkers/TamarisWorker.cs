// a.snegovoy@gmail.com

using Admitad.Converters.Handlers;

namespace Admitad.Converters.Workers.ShopWorkers
{
    internal sealed class TamarisWorker : BaseShopWorker, IShopWorker
    {
        public TamarisWorker()
        {
            _handlers.Add( new AlwaysAdultWomen() );
            _handlers.Add( new CategoryPathToModel() );
        }
    }
}