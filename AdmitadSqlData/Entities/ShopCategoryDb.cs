// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "category_shop" ) ]
    public sealed class ShopCategoryDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "id_shop" ) ]
        public int ShopId { get; set; }
        [ Column( "id_category" ) ]
        public string CategoryId { get; set; }
        [ Column( "id_parent" ) ]
        public string ParentId { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "update_date" ) ]
        public DateTime UpdateDate { get; set; }
    }
}