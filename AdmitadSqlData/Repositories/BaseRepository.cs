// a.snegovoy@gmail.com

using AdmitadSqlData.DbContexts;

namespace AdmitadSqlData.Repositories
{
    internal abstract class BaseRepository
    {

        protected readonly string ConnectionString;

        protected BaseRepository( string connectionString = null )
        {
            ConnectionString = connectionString ?? "server=185.221.152.127;user=thestore;password=moonlike-mitts-0Concord;database=theStore;convert zero datetime=True;";
        }
        
        protected ShopDbContext GetDb() => new ShopDbContext( ConnectionString );
    }
}