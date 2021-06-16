// a.snegovoy@gmail.com

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [ Table( "category_shop" ) ]
    public sealed class CategoryShopDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column("id_shop") ]
        public int ShopId { get; set; }
        [ Column("id_category") ]
        public string CategoryId { get; set; }
        [ Column("id_parent") ]
        public string ParentId { get; set; }
        [ Column("name") ]
        public string Name { get; set; }
        [ Column("men_count") ]
        public int? NumberMen { get; set; }
        [ Column("women_count") ]
        public int? NumberWomen { get; set; }
        [ Column("total_count") ]
        public int? TotalCount { get; set; }
        [ Column("update_date") ]
        public DateTime? UpdateDate { get; set; }
        [ Column("age_id") ]
        public int? AgeId { get; set; }
        [ Column("sex_id") ]
        public int? SexId { get; set; }
        public AgeDb Age { get; set; }
        public SexDb Sex { get; set; }
    }
}