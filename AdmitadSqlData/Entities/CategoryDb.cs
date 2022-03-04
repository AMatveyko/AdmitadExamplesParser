// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "category" ) ]
    public sealed class CategoryDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "id_parent" ) ]
        public int ParentId { get; set; }
        [ Column( "enabled" ) ]
        public bool Enabled { get; set; }
        [ Column( "search" ) ]
        public string Search { get; set; }
        [ Column( "search_minus" ) ]
        public string SearchExclude { get; set; }
        [ Column( "search_fields" ) ]
        public string Fields { get; set; }
        [ Column( "search_gender" ) ]
        public string Gender { get; set; }
        [ Column( "search_age" ) ]
        public int Age { get; set; }
        [ Column( "lvl" ) ]
        public int Level { get; set; }
        [ Column("search_minus_fields") ]
        public string ExcludeWordsFields { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "name_h1" ) ]
        public string NameH1 { get; set; }
        [ Column( "search_specify" ) ]
        public string SearchSpecify { get; set; }
        [ Column( "take_unisex" ) ]
        public bool TakeUnisex { get; set; }
    }
}