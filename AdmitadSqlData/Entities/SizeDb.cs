// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "size" ) ]
    public class SizeDb
    {
        [ Column( "id" ) ] public int Id { get; set; }
        [ Column( "name" ) ] public string Name { get; set; }
        [ Column( "name_sin" ) ] public string SynonymName { get; set; }
    }
}