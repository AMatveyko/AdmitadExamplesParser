// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    public class CatalogDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column("name") ]
        [ MaxLength( 16 ) ]
        public string Name { get; set; }
    }
}