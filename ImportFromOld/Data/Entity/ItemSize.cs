// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace ImportFromOld.Data.Entity
{
    [ Keyless ]
    [ Table( "item_sizes" ) ]
    public sealed class ItemSize : IItemProperty
    {
        [ Column( "id_item" ) ]
        public int OfferId { get; set; }
        [ Column( "id_size" ) ]
        public int Value { get; set; }
    }
}