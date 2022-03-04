// a.snegovoy@gmail.com

namespace Common.Entities
{
    public interface IIndexedEntities
    {
        string Id { get; }
        string RoutingId { get; }
        static string IndexName { get; }
    }
}