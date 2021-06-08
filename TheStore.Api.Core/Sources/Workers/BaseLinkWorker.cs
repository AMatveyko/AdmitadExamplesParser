// a.snegovoy@gmail.com

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal abstract class BaseLinkWorker
    {
        private readonly ElasticSearchClientSettings _settings;
        protected readonly BackgroundWorks Works;
        protected readonly DbHelper Db;

        protected BaseLinkWorker( ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper db )
        {
            _settings = settings;
            Works = works;
            Db = db;
        }

        protected ElasticSearchClient<Product> CreateElasticClient( BackgroundBaseContext context ) =>
            new ( _settings, context );

        protected ProductLinker CreateLinker( BackgroundBaseContext context ) =>
            new(_settings, Db, context);

    }
}