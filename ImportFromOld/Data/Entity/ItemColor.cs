// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace ImportFromOld.Data.Entity
{
    [ Keyless ]
    [ Table( "item_colors" ) ]
    public sealed class ItemColor : IItemProperty
    {
        [ Column( "id_item" ) ]
        public int OfferId { get; set; }
        [ Column( "id_color" ) ]
        public int Value { get; set; }
    }
}