// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "UnknownBrands" ) ]
    public class UnknownBrands
    {
        [ Column( "Id" ) ]
        public int Id { get; set; }
        [ Column( "Name" ) ]
        public string Name { get; set; }
        [ Column( "original_name" ) ]
        public string OriginalName { get; set; }
        [ Column( "NumberOfProducts" ) ]
        public long NumberOfProducts { get; set; }
    }
}