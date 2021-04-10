// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

using AdmitadExamplesParser.Entities;

using Newtonsoft.Json;

namespace AdmitadCommon.Entities
{
    public interface IProductForIndex : IIndexedEntities
    {
        [ JsonProperty ( "url" ) ] public string Url { get; }
        [ JsonProperty ( "delivery" ) ] public byte Delivery { get; }
        [ JsonProperty ( "dateUpdate" ) ] public DateTime UpdateDate { get; }
        [ JsonProperty( "name" ) ] public string Name { get; }
        [ JsonProperty( "model" ) ] public string Model { get; }
        [ JsonProperty( "typePrefix" ) ] public string TypePrefix { get; }
        [ JsonProperty( "categoryName" ) ] public string CategoryName { get; }
        [ JsonProperty ( "description" ) ] public string Description { get; }
        [ JsonProperty( "photos" ) ] public List<string> Photos { get; }
        [ JsonProperty("param") ] public string Param { get; }
        [ JsonProperty( "jsonParams" ) ] public string JsonParams { get; }
        [ JsonProperty ( "gender" ) ] public string Gender { get; }
        [ JsonProperty ( "age" ) ] public string Age { get; }
        [ JsonProperty ( "shopId" ) ] public string ShopId { get; }
        [ JsonProperty ( "brandId" ) ] public string BrandId { get; }
        [ JsonProperty ( "vendorNameClearly" ) ] public string VendorNameClearly { get; }
        [ JsonProperty ( "country" ) ] public string CountryId { get; }
        [ JsonProperty ( "price" ) ] public decimal Price { get; }
        [ JsonProperty ( "oldPrice" ) ] public decimal OldPrice { get; }
        [ JsonProperty ( "discount" ) ] public short Discount { get; }
        [ JsonProperty ( "currency" ) ] public string Currency { get; }
        [ JsonProperty ( "salesNotes" ) ] public string SalesNotes { get; }
        [ JsonProperty( "soldout" ) ] public byte Soldout { get; set; }
    }
}