// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace ImportFromOld.Data.Entity
{
    [ Keyless ]
    [ Table( "item_brands" ) ]
    public sealed class ItemBrand : IItemProperty
    {
        [ Column( "id_item" ) ]
        public int OfferId { get; set; }
        [ Column( "id_brand" ) ]
        public int Value { get; set; }

    }
}