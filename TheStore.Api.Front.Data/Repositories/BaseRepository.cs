// a.snegovoy@gmail.com

using System;

using Common.Settings;

using TheStore.Api.Front.Data.DbContexts;

namespace TheStore.Api.Front.Data.Repositories
{
    public abstract class BaseRepository : IDisposable
    {
        internal readonly TheStoreDbContext Db;
        
        protected BaseRepository( DbSettings settings )
        {
            Db = new TheStoreDbContext( settings.GetConnectionString(), settings.Version );
        }

        public void SaveChanges() => Db.SaveChanges();
        
        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}