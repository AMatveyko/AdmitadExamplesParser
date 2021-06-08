// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadSqlData.DbContexts;
using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal sealed class CountryRepository : BaseRepository
    {
        
        public CountryRepository( string connectionString, string version )
            :base( connectionString, version ) { }
        
        public List<CountryDb> GetAllCountries()
        {
            using var db = GetDb();
            return db.Countries.ToList();
        }
    }
}