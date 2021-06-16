// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace Common.Entities
{
    public class ProductPart
    {
        [ JsonProperty( "id" ) ] public string Id { get; set; }
        [ JsonIgnore ] public string RoutingId => $"R-{Id}";
        [ JsonProperty( "offerIds" ) ] public string[] OfferIds { get; set; }
        [ JsonProperty( "jsonParams" ) ] public string JsonParams { get; set; }
        [ JsonProperty( "soldout" ) ] public byte Soldout { get; set; }
        [ JsonProperty( "url" ) ] public string Url { get; set; }
    }
}