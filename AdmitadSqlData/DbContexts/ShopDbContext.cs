// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities;

using AdmitadSqlData.Entities;

using Microsoft.EntityFrameworkCore;

namespace AdmitadSqlData.DbContexts
{
    internal class ShopDbContext : DbContext
    {

        private readonly string _connectionString;
        private readonly string _version;
        
        public ShopDbContext( string connectionString, string version )
        {
            _connectionString = connectionString;
            _version = version;
        }
        
        public DbSet<Shop> Shops { get; set; }
        public DbSet<CountryDb> Countries { get; set; }
        public DbSet<CategoryDb> Categories { get; set; }
        public DbSet<TagDb> Tags { get; set; }
        public DbSet<ColorDb> Colors { get; set; }
        public DbSet<SostavDb> Materials { get; set; }
        public DbSet<SizeDb> Sizes { get; set; }
        public DbSet<BrandDb> Brands { get; set; }
        public DbSet<UnknownBrands> UnknownBrands { get; set; }
        public DbSet<OptionDb> SettingsOptions { get; set; }
        public DbSet<ProductsByCategory> ProductsByCategories { get; set; }
        public DbSet<ShopStatisticsDb> ShopStatistics { get; set; }
        public DbSet<UnknownCountry> UnknownCountries { get; set; }
        public DbSet<ShopCategoryDb> ShopCategories { get; set; }
        public DbSet<ParseLog> ParseLogs { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder ) {
            optionsBuilder.UseMySql( _connectionString,
                new MySqlServerVersion( new Version( _version ) ) );
        }
    }
}