// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public interface IIndexedEntities
    {
        string Id { get; }
        string RoutingId { get; }
        static string IndexName { get; }
    }
}