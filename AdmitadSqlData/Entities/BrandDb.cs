// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "brand" ) ]
    public class BrandDb
    {
        [ Column( "id" ) ] public int Id { get; set; }
        [ Column( "name" ) ] public string Name { get; set; }
        [ Column( "name_clearly" ) ] public string ClearlyName { get; set; }
        [ Column( "name2_clearly" ) ] public string SecondClearlyName { get; set; }
        [ Column( "duplicate" ) ] public bool Duplicate { get; set; }
        [Column("enabled")] public bool Enabled { get; set; }
    }
}