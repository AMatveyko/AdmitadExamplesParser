// a.snegovoy@gmail.com

using System.Collections.Generic;

using Newtonsoft.Json;

namespace AdmitadCommon.Entities
{
    public sealed class Param {
        
        public Param( string name, string unit ) {
            Name = name;
            Unit = unit;
        }
        
        [ JsonProperty ( "name" ) ] public string Name { get; }
        [JsonProperty("unit")] public string Unit { get; }
        [ JsonProperty( "values" ) ] public List<string> Values { get; } = new();


        public void AddValueIfNeed( string value )
        {
            if( Values.Contains( value ) == false ) {
                Values.Add( value );
            }
        }
    }
}