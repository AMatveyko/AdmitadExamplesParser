// a.snegovoy@gmail.com

using System.Collections.Generic;

using Newtonsoft.Json;

namespace Common.Entities
{
    public class ProductPart : IProductPhotos
    {
        [ JsonProperty( "id" ) ] public string Id { get; set; }
        [ JsonIgnore ] public string RoutingId => GetRouting( Id );
        [ JsonProperty( "offerIds" ) ] public string[] OfferIds { get; set; }
        [ JsonProperty( "jsonParams" ) ] public string JsonParams { get; set; }
        [ JsonProperty( "soldout" ) ] public byte Soldout { get; set; }
        [ JsonProperty( "url" ) ] public string Url { get; set; }
        [ JsonProperty( "photos" ) ] public List<string> Photos { get; set; }

        public static string GetRouting( string id = null ) => $"R-{id}";
    }
}