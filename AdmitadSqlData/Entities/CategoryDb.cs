// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "category" ) ]
    public class CategoryDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
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
        [ Column("ExcludeWordsFields") ]
        public string ExcludeWordsFields { get; set; }

    }
}