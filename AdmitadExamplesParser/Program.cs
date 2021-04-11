using System.Collections.Generic;

using AdmitadExamplesParser.Entities;
using AdmitadExamplesParser.Workers;
using AdmitadExamplesParser.Workers.Components;

using Messenger;

namespace AdmitadExamplesParser
{
    internal class Program
    {
        private static void Main(
            string[] args )
        {
            var messengerSettings = new MessengerSettings();
            messengerSettings.Clients.Add( new TelegramSettings {
                Enabled = true,
                Token = "1795605206:AAFlbA3CLKQY2b6OmBDZlClCfx2j7iDEk1c",
                ChatId = "-1001160519062"
            });
            var settings = new ProcessorSettings {
                AttemptsToDownload = 3,
                EnableExtendedStatistics = true,
                DirectoryPath = @"g:\admitadFeeds\",
                DuplicateFile = @"o:\admitad\fromZak\dublicat.xml",
                ShowStatistics = true,
                ElasticSearchClientSettings = new ElasticSearchClientSettings {
                    ElasticSearchUrl = "http://elastic.matveyko.su:9200",
                    //ElasticSearchUrl = "http://127.0.0.1:9200",
                    DefaultIndex = "products-25",
                    //DefaultIndex = "offers",
                    FrameSize = 50000,
                    ComponentForIndex = ComponentType.ElasticSearch
                },
                MessengerSettings = messengerSettings
            };
            
            
            
            var processor = new Processor( settings );
            processor.Start();
        }
    }
}