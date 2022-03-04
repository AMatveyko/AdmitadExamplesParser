// a.snegovoy@gmail.com

namespace Common.Elastic.Workers
{
    public interface IIndexProductWorker
    {
        void RemoveCategory( string productId, string categoryId );
        void RemoveTag( string productId, string tagId );
    }
}