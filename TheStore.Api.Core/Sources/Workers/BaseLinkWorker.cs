// a.snegovoy@gmail.com

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

namespace TheStore.Api.Core.Sources.Workers
{
    public abstract class BaseLinkWorker
    {
        private readonly ElasticSearchClientSettings _settings;

        protected BaseLinkWorker( ElasticSearchClientSettings settings )
        {
            _settings = settings;
        }

        protected ElasticSearchClient<Product> CreateElasticClient( BackgroundBaseContext context ) =>
            new ( _settings, context );

        protected ProductLinker CreateLinker(
            BackgroundBaseContext context ) =>
            new(_settings, context);

    }
}