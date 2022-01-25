// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace AdmitadSqlData.Entities
{
    [ Table( "shop_statistics" ) ]
    internal sealed class ShopStatisticsDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "shop_id" ) ]
        public int ShopId { get; set; }
        [ Column( "total_count" ) ]
        public int TotalCount { get; set; }
        [ Column( "soldout_count" ) ]
        public int SoldoutCount { get; set; }
        [ Column( "update_date" ) ]
        public DateTime UpdateDate { get; set; }
    }
}