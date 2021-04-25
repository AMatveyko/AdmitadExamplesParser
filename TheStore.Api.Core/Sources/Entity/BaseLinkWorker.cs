// a.snegovoy@gmail.com

using Admitad.Converters;

using AdmitadCommon.Entities;
using AdmitadCommon.Workers;

namespace TheStore.Api.Core.Sources.Entity
{
    public abstract class BaseLinkWorker
    {
        private readonly ElasticSearchClientSettings _settings;

        protected BaseLinkWorker( ElasticSearchClientSettings settings )
        {
            _settings = settings;
        }

        protected ElasticSearchClient<Product> CreateClient( BackgroundBaseContext context ) =>
            new ( _settings, context );

        protected ProductLinker CreateLinker(
            BackgroundBaseContext context ) =>
            new(_settings, context);

    }
}