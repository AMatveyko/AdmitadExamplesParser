// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "tag" ) ]
    public sealed class TagDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "pol" ) ]
        public string Pol { get; set; }
        [ Column( "search_fields" ) ]
        public string SearchFields { get; set; }
        [ Column( "enabled" ) ]
        public bool Enabled { get; set; }
        [ Column( "id_category" ) ]
        public int IdCategory { get; set; }
        [ Column( "name_specify" ) ]
        public string SpecifyWords { get; set; }
        [ Column( "exclude_phrase" ) ]
        public string ExcludePhrase { get; set; }
        [ Column( "name_title" ) ]
        public string NameTitle { get; set; }
    }
}