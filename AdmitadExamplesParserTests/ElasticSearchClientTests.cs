// a.snegovoy@gmail.com

using System.IO;

using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Extensions;

using Common.Settings;

using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public class ElasticSearchClientTests
    {
        private readonly ElasticSearchClientSettings _settings = new ElasticSearchClientSettings {
            //ElasticSearchUrl = "http://127.0.0.1:9200",
            ElasticSearchUrl = "http://185.221.152.127:9200",
            //ElasticSearchUrl = "http://127.0.0.1:8888",
            DefaultIndex = "products-old-1",
            FrameSize = 10000
        };

        [ Test ]
        public void ScrollApi()
        {
            var client = CreateClient();
            var ids = client.GetIds();
            File.WriteAllLines( @"o:\admitad\workData\scrollApi\products-old-1.txt", ids );
        }
        
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