// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Common.Settings;
using Common.SqlData.Entities;

namespace TheStore.Api.Front.Data.Repositories
{
    public sealed class UrlStatisticsRepository : BaseRepository
    {
        public UrlStatisticsRepository(
            DbSettings settings )
            : base( settings ) { }

        public List<UrlStatisticsDb> GetAll() => Db.UrlStatistics.ToList();
    }
}