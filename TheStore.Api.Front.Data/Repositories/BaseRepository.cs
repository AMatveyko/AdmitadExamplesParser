// a.snegovoy@gmail.com

using TheStore.Api.Front.Data.DbContexts;

namespace TheStore.Api.Front.Data.Repositories
{
    public abstract class BaseRepository
    {
        internal readonly TheStoreDbContext Db;
        
        protected BaseRepository( string connectionString, string version )
        {
            Db = new TheStoreDbContext( connectionString, version );
        }
        
    }
}