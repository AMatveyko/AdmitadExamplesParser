// a.snegovoy@gmail.com

namespace Common.Elastic.Workers
{
    public interface IIndexTagsWorker
    {
        long GetProductsCountWithTag(
            string tagId );
    }
}