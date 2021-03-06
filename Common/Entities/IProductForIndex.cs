// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace Common.Entities
{
    public interface IProductForIndex : IIndexedEntities, IProductRatingUpdateData
    {
        [ JsonProperty( "originalCategoryId" ) ] public string OriginalCategoryId { get; set; }
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
        [ JsonProperty ( "vendor" ) ] public string Vendor { get; }
        [ JsonProperty ( "vendorNameClearly" ) ] public string VendorNameClearly { get; }
        [ JsonProperty ( "country" ) ] public string CountryId { get; }
        [ JsonProperty ( "price" ) ] public decimal Price { get; }
        [ JsonProperty ( "oldPrice" ) ] public decimal OldPrice { get; }
        [ JsonProperty ( "discount" ) ] public short Discount { get; }
        [ JsonProperty ( "currency" ) ] public string Currency { get; }
        [ JsonProperty ( "salesNotes" ) ] public string SalesNotes { get; }
        [ JsonProperty( "offerIds" ) ] public string[] OfferIds { get; }
        [ JsonProperty( "soldout" ) ] public byte Soldout { get; set; }
        [ JsonProperty( "type" ) ] public string Type { get; set; }
        [ JsonProperty( "ageFromTxt" ) ] public string AgeFromTxt { get; set; }
        [ JsonProperty( "ageFromInt" ) ] public int? AgeFromIntFact { get; set; }
        [ JsonProperty( "ageToTxt" ) ] public string AgeToTxt { get; set; }
        [ JsonProperty( "ageToInt" ) ] public int? AgeToIntFact { get; set; }

    }
}