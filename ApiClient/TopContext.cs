// a.snegovoy@gmail.com

using System.Collections.Generic;

using Newtonsoft.Json;

namespace ApiClient
{
    public class TopContext : Context
    {
        [ JsonProperty( "contexts" ) ]
        public List<Context> Contexts { get; set; }
    }
}