// a.snegovoy@gmail.com

using AdmitadSqlData.DbContexts;

namespace AdmitadSqlData.Repositories
{
    internal abstract class BaseRepository
    {
        private readonly string _connectionString;
        private readonly string _version;

        protected BaseRepository( string connectionString, string version )
        {
            _connectionString = connectionString;
            _version = version;
        }
        
        protected ShopDbContext GetDb() => new ShopDbContext( _connectionString, _version );
    }
}