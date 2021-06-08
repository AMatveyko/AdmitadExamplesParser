// a.snegovoy@gmail.com

using ImportFromOld.Data.Entity;

using Microsoft.EntityFrameworkCore;

namespace ImportFromOld.Data
{
    public sealed class ImportDbContext : DbContext
    {
        
        public ImportDbContext()
        {
            Database.EnsureCreated();
            Database.SetCommandTimeout( 36000 );
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=GT;Database=shops;Trusted_Connection=True;Connection Timeout=36000;");
        }
        
        public DbSet<OfferOld> Offers { get; set; }
        public DbSet<ItemBrand> Brands { get; set; }
        public DbSet<ItemTag> Tags { get; set; }
        public DbSet<ItemColor> Colors { get; set; }
        public DbSet<ItemSize> Sizes { get; set; }
        public DbSet<ItemCountry> Countries { get; set; }
        public DbSet<ItemMaterial> Materials { get; set; }
        
    }
}