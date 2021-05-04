// a.snegovoy@gmail.com

using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Extensions;

using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public class ElasticSearchClientTests
    {
        private readonly ElasticSearchClientSettings _settings = new ElasticSearchClientSettings {
            //ElasticSearchUrl = "http://127.0.0.1:9200",
            ElasticSearchUrl = "http://elastic.matveyko.su:9200",
            //ElasticSearchUrl = "http://127.0.0.1:8888",
            DefaultIndex = "products-25",
            FrameSize = 10000
        };

        [ Test ]
        public void GetCountAllDocuments()
        {
            var client = CreateClient();
            var count = client.GetCountAllDocuments();
        }
        
        private ElasticSearchClient<Product> CreateClient( string indexName = null )
        {
            if( indexName.IsNotNullOrWhiteSpace() ) {
                _settings.DefaultIndex = indexName;
            }
            return new ( _settings, new BackgroundBaseContext("1", "name" ) );
        }
    }
}