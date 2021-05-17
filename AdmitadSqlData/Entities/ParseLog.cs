// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "shop_parse_log" ) ]
    public sealed class ParseLog
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "date_parse" ) ]
        public DateTime Date { get; set; }
        [ Column( "id_shop" ) ]
        public int ShopId { get; set; }
        [ Column( "xml_size" ) ]
        public long FileSize { get; set; }
        [ Column( "offer_cnt" ) ]
        public int OfferCount { get; set; }
        [ Column( "offer_deleted_cnt" ) ]
        public int SoldOutOfferCount { get; set; }
        [ Column( "category_cnt" ) ]
        public int CategoryCount { get; set; }
        [ Column( "download_time" ) ]
        public long DownloadTime { get; set; }
    }
}