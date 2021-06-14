// a.snegovoy@gmail.com

using System;
using System.Linq;

using Admitad.Converters;
using Admitad.Converters.Helpers;
using Admitad.Converters.Workers;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;
using Common.Settings;
using Common.Workers;

using NUnit.Framework;

using TheStore.Api.Core.Sources.Workers;

namespace AdmitadExamplesParserTests
{
    public sealed class ProcessingVersion2Tests
    {
        
        private readonly ElasticSearchClientSettings _settings = new ElasticSearchClientSettings {
            // ElasticSearchUrl = "http://127.0.0.1:9200",
            ElasticSearchUrl = "http://185.221.152.127:9200",
            // ElasticSearchUrl = "http://127.0.0.1:8888",
            DefaultIndex = "products-1",
            //ElasticSearchUrl = "http://10.1.2.11:9200",
            //DefaultIndex = "products-dev-1",
            FrameSize = 10000
        };
        
        [ Test ]
        public void DownloadFeed()
        {
            var dbSettings = SettingsBuilder.GetDbSettings();
            var shopInfo = new DbHelper( dbSettings ).GetShop( 120 );
            shopInfo.VersionProcessing = 2;
            var downloader = new FeedsDownloader(
                3,
                new DbHelper( dbSettings ),
                new BackgroundBaseContext( "-", "name" ) );
            var info =downloader.Download( "tests", shopInfo );
        }

        [ Test ]
        public void ParseOffers()
        {
            var downloadInfo = GetDownloadInfo();
            var parser = new GeneralParser( downloadInfo, new BackgroundBaseContext( "1", "1" ) );
            var offers = parser.Parse();
            var ids = offers.DeletedOffers.Select( o => o.OfferId.ToLower() ).ToArray();
            var client = IndexClient.Create( _settings, new BackgroundBaseContext("1","1") );
            var products = client.SearchProductsByOffersIds( ids );
        }

        [ Test ]
        public void GetProductsMultiGet()
        {
            var client = IndexClient.Create( _settings, new BackgroundBaseContext("1","1") );
            var result = client.GetProductsByIds( new []{ "b264ec44406c9457861b314165d6bf52", "7f5edf239c7b7700defb8fcad1408817" } );
        }

        [ Test ]
        public void CheckEndFile()
        {
            var path = @"o:\AdmitadExamplesParser\AdmitadExamplesParserTests\lamoda.xml";
            var result = FileChecker.WithAnEnd( path );
        }
        
        [ Test ]
        public void DeleteOffersTest() {
            var dbHelper = new DbHelper( SettingsBuilder.GetDbSettings() );
            var downloadInfo = GetDownloadInfo( dbHelper );
            var handler = new ShopChangesHandler(
                new ProcessShopContext( "33", 33, downloadInfo, false ),
                _settings,
                dbHelper );
            handler.Process();
        }

        private static DownloadInfo GetDownloadInfo( DbHelper dbHelper = null )
        {
            //var lastUpdate =   : dbHelper.GetShop( 33 );
            var xmlInfo = dbHelper == null
                ? new XmlFileInfo( "Lamoda", "lamoda", "", 33, 10, 2, DateTime.Now )
                : dbHelper.GetShop( 33 );
            var downloadInfo = new DownloadInfo( xmlInfo );
            downloadInfo.StartTime = DateTime.Parse( "08-Jun-21 3:20:05" );
            downloadInfo.Error = DownloadError.Ok;
            downloadInfo.DownloadTime = 1189;
            downloadInfo.FilePath = @"o:\AdmitadExamplesParser\AdmitadExamplesParserTests\lamoda.xml";
            return downloadInfo;
        }

    }
}