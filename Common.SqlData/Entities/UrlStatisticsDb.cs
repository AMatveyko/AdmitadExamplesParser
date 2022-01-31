// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.SqlData.Entities
{
    [Table("listing_url")]
    public sealed class UrlStatisticsDb
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("url")]
        public string Url { get; set; }
    }
}