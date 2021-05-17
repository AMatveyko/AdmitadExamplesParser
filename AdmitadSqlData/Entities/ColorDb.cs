// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "color" ) ]
    public class ColorDb : ISearchNamesDb
    {
        [ Column( "id" ) ] public int Id { get; set; }
        [ Column( "id_parent" ) ] public int ParentId { get; set; }
        [ Column( "name" ) ] public string Name { get; set; }
        [ Column( "name2" ) ] public string Name2 { get; set; }
        [ Column( "name3" ) ] public string Name3 { get; set; }
        [ Column( "name4" ) ] public string Name4 { get; set; }
        [ Column( "name_sin1" ) ] public string SynonymName { get; set; }
        [ Column( "name_search" ) ] public string SearchNames { get; set; }
    }
}