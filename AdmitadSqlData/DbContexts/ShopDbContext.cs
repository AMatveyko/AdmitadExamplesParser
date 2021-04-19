// a.snegovoy@gmail.com

using System;

using AdmitadSqlData.Entities;

using Microsoft.EntityFrameworkCore;

namespace AdmitadSqlData.DbContexts
{
    internal class ShopDbContext : DbContext
    {

        private readonly string _connectionString;
        
        public ShopDbContext( string connectionString )
        {
            _connectionString = connectionString;
        }
        
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CategoryDb> Categories { get; set; }
        public DbSet<TagDb> Tags { get; set; }
        public DbSet<ColorDb> Colors { get; set; }
        public DbSet<SostavDb> Materials { get; set; }
        public DbSet<SizeDb> Sizes { get; set; }
        public DbSet<BrandDb> Brands { get; set; }
        public DbSet<UnknownBrands> UnknownBrands { get; set; }
        public DbSet<OptionDb> SettingsOptions { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder ) {
            optionsBuilder.UseMySql( _connectionString,
                new MySqlServerVersion( new Version( "10.3.27" ) ) );
        }
    }
}