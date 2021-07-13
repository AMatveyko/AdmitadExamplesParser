// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [ Table( "tag_shopsy" ) ]
    public sealed class OtherTagDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "id_category" ) ]
        public int CategoryId { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "title" ) ]
        public string Title { get; set; }
        [ Column( "title_genitive" ) ]
        public string Genitive { get; set; }
        [ Column( "title_accusative" ) ]
        public string Accusative { get; set; }
        [ Column( "value" ) ]
        public string Value { get; set; }
        [ Column( "extended" ) ]
        public int Extended { get; set; }
        [ Column( "priority" ) ]
        public int Priority { get; set; }
        [ Column( "type_id" ) ]
        public int TypeId { get; set; }
        [ Column( "type_name" ) ]
        public string TypeName { get; set; }
        [ Column("title_en" ) ]
        public string TitleEn { get; set; }
        [ Column( "name_en" ) ]
        public string NameEn { get; set; }
        [ Column( "value_en" ) ]
        public string ValueEn { get; set; }
        [ Column( "url" ) ]
        public string Url { get; set; }
        [ Column( "rules" ) ]
        public string Rules { get; set; }
        [ Column( "unit" ) ]
        public string Unit { get; set; }
        [ Column( "unit_value" ) ]
        public string UnitValue { get; set; }
        [ Column( "enabled" ) ]
        public bool Enabled { get; set; }
        [ Column( "count" ) ]
        public int Count { get; set; }
        [ Column( "created_at" ) ]
        public DateTime CreatedAt { get; set; }
        [ Column( "verified" ) ]
        public bool Verified { get; set; }
        [ Column( "duplicate" ) ]
        public bool Duplicate { get; set; }
    }
}