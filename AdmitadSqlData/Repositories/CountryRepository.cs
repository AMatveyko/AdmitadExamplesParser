// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using AdmitadSqlData.DbContexts;
using AdmitadSqlData.Entities;

namespace AdmitadSqlData.Repositories
{
    internal sealed class CountryRepository : BaseRepository
    {
        public List<Country> GetAllCountries()
        {
            using var db = GetDb();
            return db.Countries.ToList();
        }

        public CountryRepository(
            string connectionString = null )
            : base( connectionString ) { }
    }
}