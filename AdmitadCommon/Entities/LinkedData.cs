// a.snegovoy@gmail.com

using AdmitadExamplesParser.Entities;

using Nest;

using Newtonsoft.Json;

namespace AdmitadCommon.Entities
{
    public class LinkedData : IIndexedEntities
    {
        [ JsonProperty( "id" ) ] public string Id => $"LD-{ParentId}";
        [ JsonIgnore ]
        public string RoutingId => $"R-{ParentId}";
        [ JsonIgnore ]
        public string ParentId { get; set; }
        [ JsonProperty( "categories" ) ] public string[] Categories { get; set; }
        [ JsonProperty ( "tags" ) ]
        public string[] Tags { get; set; }
        [ JsonProperty ( "colors" ) ]
        public string[] Colors { get; set; }
        [ JsonProperty ( "materials" ) ]
        public string[] Materials { get; set;}
        [ JsonProperty ( "sizes" ) ]
        public string[] Sizes { get; set; }
        [ JsonProperty( "soldout" ) ] public bool Soldout { get; set; } = false;
        [ JsonProperty( "click" ) ] public int Click { get; set; } = 0; 

        public JoinField MyJoinField { get; set; }
    }
}