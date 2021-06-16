// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [ Table( "shop" ) ]
    public sealed class ShopDb
    {
        [ Column("id") ]
        public int Id { get; set; }
        [ Column("name") ]
        public string Name { get; set; }
        [ Column("name_lat") ]
        public string LatinName { get; set; }
        [ Column("url") ]
        public string Url { get; set; }
        [ Column("description") ]
        public string Description { get; set; }
        [ Column("xml_feed") ]
        public string XmlFeed { get; set; }
        [ Column("date_update") ]
        public DateTime DateUpdate { get; set; }
        [ Column("enabled") ]
        public bool Enabled { get; set; }
        [ Column("partner_url") ]
        public string PartnerUrl { get; set; }
        [ Column("logo") ]
        public string Logo { get; set; }
        [ Column("weight") ]
        public int Weight { get; set; }
        [ Column("version_processing") ]
        public int VersionProcessing { get; set; }
    }
}