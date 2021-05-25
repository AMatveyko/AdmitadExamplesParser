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

        public TheStoreDbContext(
            string connectionString,
            string version )
        {
            _connectionString = connectionString;
            _version = version;
        }

        public DbSet<CompareListingDb> CompareListings { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder ) {
            optionsBuilder.UseMySql( _connectionString,
                new MySqlServerVersion( new Version( _version ) ) );
        }
    }
}