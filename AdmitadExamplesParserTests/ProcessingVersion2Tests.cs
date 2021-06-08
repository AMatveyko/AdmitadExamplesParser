// a.snegovoy@gmail.com

using System;

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Workers;

using NUnit.Framework;

namespace AdmitadExamplesParserTests
{
    public sealed class ProcessingVersion2Tests
    {
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
            var xmlInfo = new XmlFileInfo( "Lamoda", "lamoda", "", 33, 10, 2, DateTime.Now );
            var downloadInfo = new DownloadInfo( xmlInfo );
            downloadInfo.StartTime = DateTime.Parse( "08-Jun-21 3:20:05" );
            downloadInfo.Error = DownloadError.Ok;
            downloadInfo.DownloadTime = 1189;
            downloadInfo.FilePath = @"o:\AdmitadExamplesParser\AdmitadExamplesParserTests\bin\Debug\net5.0\lamoda.xml";

            var parser = new GeneralParser( downloadInfo, new BackgroundBaseContext( "1", "1" ) );
            var offers = parser.Parse();
        }
    }
}