// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "sostav" ) ]
    public class SostavDb
    {
        [ Column( "id" ) ] public int Id { get; set; }
        [ Column( "name" ) ] public string Name { get; set; }
        [ Column( "name_sin" ) ] public string SynonymName { get; set; }
        [ Column( "name2" ) ] public string Name2 { get; set; }
        [ Column( "name3" ) ] public string Name3 { get; set; }
    }
}