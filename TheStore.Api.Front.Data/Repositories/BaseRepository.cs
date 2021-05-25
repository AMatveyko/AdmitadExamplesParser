// a.snegovoy@gmail.com

using TheStore.Api.Front.Data.DbContexts;

namespace TheStore.Api.Front.Data.Repositories
{
    public abstract class BaseRepository
    {
        internal readonly TheStoreDbContext DB;
        
        protected BaseRepository( string connectionString, string version )
        {
            DB = new TheStoreDbContext( connectionString, version );
        }
        
    }
}