// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportFromOld.Data.Entity
{
    [ Table( "items" ) ]
    public class OfferOld
    {
        [ Key ]
        [ Column( "aid" ) ]
        public long Id { get; set; }
        [ Column( "id" ) ]
        public string OfferId { get; set; }
        [ Column( "date_add" ) ]
        public int? AddDate { get; set; }
        [ Column( "date_update" ) ]
        public int? UpdateDate { get; set; }
        [ Column( "id_shop" ) ]
        public Int16? ShopId { get; set; }
        [ Column( "id_category" ) ]
        public int? CategoryId { get; set; }
        [ Column( "id_category2" ) ]
        public int? CategoryIdSecond { get; set; }
        [ Column( "model" ) ]
        public string Model { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "description" ) ]
        public string Description { get; set; }
        [ Column( "fullProductName" ) ]
        public string FullProductName { get; set; }
        [ Column( "size" ) ]
        public string Size { get; set; }
        [ Column( "color" ) ]
        public string Color { get; set; }
        [ Column( "sostav" ) ]
        public string Material { get; set; }
        [ Column( "param" ) ]
        public string ParamJson { get; set; }
        [ Column( "param2" ) ]
        public string Param { get; set; }
        [ Column( "available" ) ]
        public string Available { get; set; }
        [ Column( "group_id" ) ]
        public int? GroupId { get; set; }
        [ Column( "photo1" ) ]
        public string Photo1 { get; set; }
        [ Column( "photo2" ) ]
        public string Photo2 { get; set; }
        [ Column( "photo3" ) ]
        public string Photo3 { get; set; }
        [ Column( "photo4" ) ]
        public string Photo4 { get; set; }
        [ Column( "photo5" ) ]
        public string Photo5 { get; set; }
        [ Column( "photo6" ) ]
        public string Photo6 { get; set; }
        [ Column( "photo7" ) ]
        public string Photo7 { get; set; }
        [ Column( "photo8" ) ]
        public string Photo8 { get; set; }
        [ Column( "photo9" ) ]
        public string Photo9 { get; set; }
        [ Column( "country_of_origin" ) ]
        public string CountryOfOrigin { get; set; }
        [ Column( "gender" ) ]
        public string OriginalGender { get; set; }
        [ Column( "gender2" ) ]
        public string Gender { get; set; }
        [ Column( "adult" ) ]
        public Int16 Adult { get; set; }
        [ Column( "age" ) ]
        public Int16 Age { get; set; }
        [ Column( "currencyId" ) ]
        public string CurrencyId { get; set; }
        [ Column( "categoryName" ) ]
        public string CategoryName { get; set; }
        [ Column( "delivery" ) ]
        public string Delivery { get; set; }
        [ Column( "delivery-options" ) ]
        public string DeliveryOptions { get; set; }
        [ Column( "categoryId" ) ]
        public string OriginalCategoryId { get; set; }
        [ Column( "product_type" ) ]
        public string ProductType { get; set; }
        [ Column( "type" ) ]
        public string Type { get; set; }
        [ Column( "typePrefix" ) ]
        public string TypePrefix { get; set; }
        [ Column( "market_category" ) ]
        public string MarketCategory { get; set; }
        [ Column( "vendor" ) ]
        public string Vendor { get; set; }
        [ Column( "vendor_clearly" ) ]
        public string ClearlyVendorName { get; set; }
        [ Column( "vendorCode" ) ]
        public string VendorCode { get; set; }
        [ Column( "oldprice" ) ]
        public int? OldPrice { get; set; }
        [ Column( "price" ) ]
        public int? Price { get; set; }
        [ Column( "discount" ) ]
        public int? Discount { get; set; }
        [ Column( "sales_notes" ) ]
        public string SalesNotes { get; set; }
        [ Column( "local_delivery_cost" ) ]
        public int? LocalDeliveryCost { get; set; }
        [ Column( "shipping" ) ]
        public string Shipping { get; set; }
        [ Column( "store" ) ]
        public string Store { get; set; }
        [ Column( "pickup" ) ]
        public string Pickup { get; set; }
        [ Column( "topseller" ) ]
        public string TopSeller { get; set; }
        [ Column( "modified_time" ) ]
        public int? ModifiedTime { get; set; }
        [ Column( "url" ) ]
        public string Url { get; set; }
        [ Column( "rating" ) ]
        public int Rating { get; set; }
        [ Column( "view" ) ]
        public int View { get; set; }
        [ Column( "click" ) ]
        public int Click { get; set; }
        [ Column( "enabled" ) ]
        public Int16 Enabled { get; set; }
    }
}