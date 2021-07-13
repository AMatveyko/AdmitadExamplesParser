// a.snegovoy@gmail.com

using Common.Settings;

using NUnit.Framework;

using TheStore.Api.Core.Sources.Workers;

namespace AdmitadExamplesParserTests
{
    public class ControllersTests
    {
        
        private readonly ElasticSearchClientSettings _settings = new ElasticSearchClientSettings {
            //ElasticSearchUrl = "http://127.0.0.1:9200",
            ElasticSearchUrl = "http://185.221.152.127:9200",
            //ElasticSearchUrl = "http://127.0.0.1:8888",
            DefaultIndex = "products-1",
            FrameSize = 10000
        };
        
        [ Test ]
        public void RemoveProductFromCategory()
        {
            var productId = "96e717d9aeba2ff1b9f791621f4ed4d6";
            var worker = new ProductWorker( productId, _settings );
            worker.RemoveFromCategory( "10122010" );
        }
    }
}