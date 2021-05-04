// a.snegovoy@gmail.com

using System.Collections.Generic;

using Newtonsoft.Json;

namespace ApiClient
{
    public class Context
    {
        [ JsonProperty( "id" ) ]
        public string Id { get; set; }
        [ JsonProperty( "name" ) ]
        public string Name { get; set; }
        [ JsonProperty( "status" ) ]
        public string Status { get; set; }
        [ JsonProperty( "isFinished" ) ]
        public bool IsFinished { get; set; }
        [ JsonProperty( "percentFinished" ) ]
        public int PercentFinished { get; set; }
        [ JsonProperty( "content" ) ]
        public string Content { get; set; }
        [ JsonProperty( "isError" ) ]
        public bool IsError { get; set; }
        [ JsonProperty( "time" ) ]
        public long Time { get; set; }
        [ JsonProperty( "messages" ) ]
        public List<string> Messages { get; set; }
    }
}