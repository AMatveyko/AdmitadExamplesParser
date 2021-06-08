// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [ Table( "compare_listings" ) ]
    public sealed class CompareListingDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "date_add" ) ]
        public DateTime? AddDate { get; set; }
        [ Column( "url" ) ]
        public string Url { get; set; }
        [ Column( "visits" ) ]
        public int Visits { get; set; }
        [ Column( "old_cnt" ) ]
        public int? OldSiteProductCount { get; set; }
        [ Column( "old_shop" ) ]
        public int? OldSiteShopCount { get; set; }
        [ Column( "new_cnt_without_analyze" ) ]
        public int? NewSiteProductCount { get; set; }
        [ Column( "new_shop_without_analyze" ) ]
        public int? NewSiteShopCount { get; set; }
    }
}