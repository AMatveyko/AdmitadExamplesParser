// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Common.SqlData.Entities;

namespace AdmitadSqlData.Entities
{
    [ Table("shop" ) ]
    internal sealed class Shop
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "name_lat" ) ]
        public string NameLatin { get; set; }
        [ Column( "enabled" ) ]
        public bool Enabled { get; set; }
        [ Column( "date_update" ) ]
        public DateTime? UpdateDate { get; set; }
        [ Column( "weight" ) ]
        public int Weight { get; set; }
        [ Column( "version_processing" ) ]
        public byte VersionProcessing { get; set; }
        public ICollection<ShopFeedDb> ShopFeeds { get; set; }
    }
}