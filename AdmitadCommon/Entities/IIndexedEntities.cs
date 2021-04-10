// a.snegovoy@gmail.com

namespace AdmitadExamplesParser.Entities
{
    public interface IIndexedEntities
    {
        string Id { get; }
        string RoutingId { get; }
        static string IndexName { get; }
    }
}