// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace AdmitadExamplesParser.Entities
{
    internal sealed class IndexEntity {

        public IndexEntity() { }

        public IndexEntity( string indexName, int docId ) {
            IndexSettings = new IndexSettings( indexName, docId );
        }
        
        [ JsonProperty( "index" ) ]
        public IndexSettings IndexSettings { get; set; }
    }

    internal sealed class IndexSettings
    {
        
        public IndexSettings() {}

        public IndexSettings( string indexName, int docId ) {
            IndexName = indexName;
            DocId = docId;
        }
        
        [ JsonProperty( "_index" ) ]
        public string IndexName { get; set; }
        [ JsonProperty( "_type" ) ]
        public string IndexType { get; } = "_doc";
        [ JsonProperty( "_id" ) ]
        public int DocId { get; set; }
    }
}