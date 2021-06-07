// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace AdmitadCommon.Entities
{
    public sealed class Product : IProductForIndex
    {
        [ JsonProperty( "view" ) ] public long View { get; set; }
        [ JsonProperty( "categories" ) ] public string[] Categories { get; set; }
        [ JsonProperty( "originalCategoryId" ) ] public string OriginalCategoryId { get; set; }
        [ JsonProperty( "tags" ) ] public string[] Tags { get; set; }
        [ JsonProperty( "colors" ) ] public string[] Colors { get; set; }
        [ JsonProperty( "materials" ) ] public string[] Materials { get; set; }
        [ JsonProperty( "sizes" ) ] public string[] Sizes { get; set; }
        [ JsonProperty( "soldout" ) ] public byte Soldout { get; set; }
        [ JsonProperty( "enable" ) ] public byte Enable { get; set; }
        [ JsonProperty( "click" ) ] public long Click { get; set; }
        [ JsonProperty( "id" ) ] public string Id { get; set; }
        [ JsonIgnore ] public string RoutingId => $"R-{Id}";
        [ JsonProperty( "url" ) ] public string Url { get; set; }
        [ JsonProperty( "delivery" ) ] public byte Delivery { get; set; }
        [ JsonProperty( "dateUpdate" ) ] public DateTime UpdateDate { get; set; }
        [ JsonProperty( "name" ) ] public string Name { get; set; }
        [ JsonProperty( "model" ) ] public string Model { get; set; }
        [ JsonProperty( "typePrefix" ) ] public string TypePrefix { get; set; }
        [ JsonProperty( "categoryName" ) ] public string CategoryName { get; set; }
        [ JsonProperty( "description" ) ] public string Description { get; set; }
        [ JsonProperty( "photos" ) ] public List<string> Photos { get; set; }
        [ JsonProperty( "param" ) ] public string Param { get; set; }
        [ JsonProperty( "jsonParams" ) ] public string JsonParams { get; set; }
        [ JsonIgnore ] public List<string> Params { get; set; }
        [ JsonProperty( "gender" ) ] public string Gender { get; set; }
        [ JsonProperty( "age" ) ] public string Age { get; set; }
        [ JsonProperty( "shopId" ) ] public string ShopId { get; set; }

        [ JsonProperty( "brandId" ) ] public string BrandId { get; set; }
        [ JsonProperty( "vendorNameClearly" ) ]
        public string VendorNameClearly { get; set; }
        [ JsonProperty ( "vendor" ) ] public string Vendor { get; set; }
        [ JsonProperty( "country" ) ] public string CountryId { get; set; }
        [ JsonProperty( "price" ) ] public decimal Price { get; set; }
        [ JsonProperty( "oldPrice" ) ] public decimal OldPrice { get; set; }
        [ JsonProperty( "discount" ) ] public short Discount { get; set; }
        [ JsonProperty( "currency" ) ] public string Currency { get; set; }
        [ JsonProperty( "salesNotes" ) ] public string SalesNotes { get; set; }
        [ JsonProperty( "offerIds" ) ] public string[] OfferIds { get; set; }
        [ JsonProperty( "rating" ) ] public int Rating { get; set; }
    }
}