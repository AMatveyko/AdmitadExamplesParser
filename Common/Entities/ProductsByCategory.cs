// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Entities
{
    [ Table( "products_by_category" ) ]
    public sealed class ProductsByCategory
    {
        [ Key ]
        [ Column( "category_id" ) ]
        public int CategoryId { get; set; }
        [ Column( "products_before_linking" ) ]
        public int ProductsBeforeLinking { get; set; }
        [ Column( "products_after_linking" ) ]
        public int ProductsAfterLinking { get; set; }
        [ Column("category_name" ) ]
        public string CategoryName { get; set; }
        [ Column( "update_date" ) ]
        public DateTime UpdateDate { get; set; }
        
    }
}