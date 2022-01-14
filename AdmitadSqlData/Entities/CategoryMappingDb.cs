using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitadSqlData.Entities
{
    [Table("category_mapping")]
    public sealed class CategoryMappingDb
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("id_shop")]
        public int ShopId { get; set; }
        [Column("id_category_shop")]
        public string ShopCategoryId { get; set; }
        [Column("id_category_thestore")]
        public int LocalCategoryId { get; set; }
    }
}
