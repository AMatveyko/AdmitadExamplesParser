// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace ImportFromOld.Data.Entity
{
    [ Keyless ]
    [ Table( "item_tags" ) ]
    public sealed class ItemTag : IItemProperty
    {
        [ Column( "id_item" ) ]
        public int OfferId { get; set; }
        [ Column( "id_tag" ) ]
        public int Value { get; set; }
    }
}