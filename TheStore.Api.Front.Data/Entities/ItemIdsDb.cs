using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [Table("item_ids")]
    public sealed class ItemIdsDb {
        [Column("id_hash")]
        public string ProductId { get; set; }
        [Column("views")]
        public int Views { get; set; }
        [Column("clicks")]
        public int Clicks { get; set; }
        [Key]
        [Column("id")]
        public int Id { get; set; }
    }
}
