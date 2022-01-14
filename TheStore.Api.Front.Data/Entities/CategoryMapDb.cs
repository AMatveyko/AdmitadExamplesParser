using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TheStore.Api.Front.Data.Entities
{
    [Table("category_mapping")]
    public sealed class CategoryMapDb
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("id_shop")]
        public int ShopId { get; set; }
        [Column("id_category_shop")]
        public string ShopCategoryId { get; set; }
        [Column("id_category_thestore")]
        public int LocalCategoryId { get; set; }
        [Column("original_name")]
        public string OriginalName { get; set; }
        [Column("original_parent_id")]
        public string OriginalParentId { get; set; }
    }
}
