// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "shop_statistics" ) ]
    internal sealed class ShopStatisticsDb
    {
        [ Key ]
        [ Column( "shop_id" ) ]
        public int ShopId { get; set; }
        [ Column( "total_before" ) ]
        public int TotalBefore { get; set; }
        [ Column( "total_after" ) ]
        public int TotalAfter { get; set; }
        [ Column( "soldout_before" ) ]
        public int SoldoutBefore { get; set; }
        [ Column( "soldout_after" ) ]
        public int SoldoutAfter { get; set; }
        [ Column( "error" ) ]
        public string Error { get; set; }
        [ Column( "update_date" ) ]
        public DateTime UpdateDate { get; set; }
    }
}