﻿// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdmitadCommon.Entities
{
    [ Serializable ]
    [ XmlRoot( "offer" ) ]
    public class RawOffer
    {
        [ XmlAttribute( "available" ) ] public string AvailableFromXml { get; set; }
        [ XmlAttribute( "deleted" ) ] public bool IsDeleted { get; set; }
        [ XmlIgnore ] public bool Available => AvailableFromXml == "true" || AvailableFromXml == "available for order";
        //[ XmlAttribute( "group_id" ) ] public int? GroupId { get; set; }
        [ XmlAttribute( "id" ) ] public string OfferId { get; set; }
        [ XmlAttribute( "type" ) ] public string Type { get; set; }
        [ XmlElement( "categoryId" ) ] public string CategoryId { get; set; }
        [ XmlElement( "currencyId" ) ] public string CurrencyId { get; set; }
        [ XmlElement( "delivery" ) ] public bool? IsDelivered { get; set; }
        [ XmlElement( "description" ) ] public string Description { get; set; }
        [ XmlElement( "sales_notes" ) ] public string SalesNotes { get; set; }
        [ XmlElement( "typePrefix" ) ] public string TypePrefix { get; set; }
        [ XmlElement( "market_category" ) ] public string MarketCategory { get; set; }
        [ XmlElement( "model" ) ] public string Model { get; set; }
        [ XmlElement( "modified_time" ) ] public string ModifiedTime { get; set; }
        [ XmlElement( "name" ) ] public string Name { get; set; }
        [ XmlElement( "price" ) ] public string Price { get; set; }
        [ XmlElement( "oldPrice" ) ] public string OldPrice { get; set; }
        [ XmlElement( "param" ) ] public List<RawParam> Params { get; set; }
        [ XmlElement( "pickup" ) ] public bool? IsPickup { get; set; }
        [ XmlElement( "picture" ) ] public List<string> Pictures { get; set; }
        [ XmlElement( "url" ) ] public string Url { get; set; }
        [ XmlElement( "vendor" ) ] public string Vendor { get; set; }
        [ XmlElement( "vendorCode" ) ] public string VendorCode { get; set; }
        [ XmlElement( "country_of_origin" ) ] public string Country { get; set; }
        [ XmlElement( "gender" ) ] public string Gender { get; set; }
        [ XmlIgnore ] public string CategoryPath { get; set; }
        [ XmlIgnore ] public string ShopName { get; set; }
        [ XmlIgnore ] public DateTime UpdateTime { get; set; }
        [ XmlIgnore ] public string ShopNameLatin { get; set; }
        [ XmlIgnore ] public string Text { get; set; }
    }
}