using System.Collections.Generic;

using AdmitadExamplesParser.Entities;
using AdmitadExamplesParser.Workers;
using AdmitadExamplesParser.Workers.Components;

namespace AdmitadExamplesParser
{
    internal class Program
    {
        private static void Main(
            string[] args )
        {
            var settings = new ProcessorSettings {
                AttemptsToDownload = 3,
                EnableExtendedStatistics = true,
                DirectoryPath = @"g:\admitadFeeds\",
                DuplicateFile = @"o:\admitad\fromZak\dublicat.xml",
                ShowStatistics = true,
                ElasticSearchClientSettings = new ElasticSearchClientSettings {
                    ElasticSearchUrl = "http://elastic.matveyko.su:9200",
                    //ElasticSearchUrl = "http://127.0.0.1:9200",
                    //DefaultIndex = "products",
                    //DefaultIndex = "offers",
                    FrameSize = 50000,
                    ComponentForIndex = ComponentType.ElasticSearch
                }
            };

            var processor = new Processor( settings );
            processor.Start();
        }
    }
}