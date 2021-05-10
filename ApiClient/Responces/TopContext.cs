// a.snegovoy@gmail.com

using System.Collections.Generic;

using Newtonsoft.Json;

namespace ApiClient.Responces
{
    public class TopContext : Context
    {
        [ JsonProperty( "contexts" ) ]
        public List<Context> Contexts { get; set; }
    }
}