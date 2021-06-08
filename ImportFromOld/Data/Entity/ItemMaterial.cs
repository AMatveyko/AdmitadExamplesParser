// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace ImportFromOld.Data.Entity
{
    [ Keyless ]
    [ Table( "item_materials" ) ]
    public sealed class ItemMaterial : IItemProperty
    {
        [ Column( "id_item" ) ]
        public int OfferId { get; set; }
        [ Column( "id_sostav" ) ]
        public int Value { get; set; }
    }
}