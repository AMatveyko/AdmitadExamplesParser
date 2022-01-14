// a.snegovoy@gmail.com

using Common.Entities;

namespace Admitad.Converters.Workers
{
    public interface IPostWorker
    {
        public void Process(
            Product product );
    }
}