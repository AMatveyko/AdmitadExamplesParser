// a.snegovoy@gmail.com

using System;

using Microsoft.EntityFrameworkCore;

using TheStore.Api.Front.Data.Entities;

namespace TheStore.Api.Front.Data.DbContexts
{
    internal class TheStoreDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly string _version;

        public TheStoreDbContext( string connectionString, string version )
        {
            _connectionString = connectionString;
            _version = version;
        }

        public DbSet<CompareListingDb> CompareListings { get; set; }
        public DbSet<ProxyDb> Proxies { get; set; }
        public DbSet<CoreSettingDb> CoreSettings { get; set; }
        public DbSet<CategoryShopDb> ShopCategories { get; set; }
        public DbSet<SexDb> Sex { get; set; }
        public DbSet<AgeDb> Ages { get; set; }
        public DbSet<ShopDb> Shops { get; set; }
        public DbSet<TagDb> Tags { get; set; }
        public DbSet<OtherTagDb> OtherTags { get; set; }
        public DbSet<CategoryDb> Categories { get; set; }
        public DbSet<ItemIds> ItemCtrs { get; set; }
        public DbSet<CategoryMapDb> CategoryMaps { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder ) {
            optionsBuilder.UseMySql( _connectionString,
                new MySqlServerVersion( new Version( _version ) ) );
        }
    }
}