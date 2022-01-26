// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace Common.SqlData.Entities
{
    [Table("shop_feeds")]
    public sealed class ShopFeedDb
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("shop_id")]
        public int ShopId { get; set; }
        [Column("url")]
        public string Url { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}