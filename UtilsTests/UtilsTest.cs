using System;
using System.IO;
using System.Linq;

using AdmitadCommon.Entities;

using Common.Api;
using Common.Helpers;
using Common.Settings;

using Messenger;

using NUnit.Framework;

using TheStore.Api.Core.Sources.Entities;
using TheStore.Api.Core.Sources.Workers;
using TheStore.Api.Front.Data.Repositories;

using Web.Common.Helpers;

namespace UtilsTests
{
    public class UtilsTest
    {
        private TelegramSettings _telegramSettings = new() {
            Enabled = true,
            Token = "sldfjaldfjad",
            ChatId = "sdfaslkfjsladf"
        };

        [ Test ]
        public void TestUserAgents()
        {
            var agent1 = UserAgents.Get();
            var agent2 = UserAgents.Get();
            Assert.AreNotEqual( agent1, agent2 );
        }
        
        [ Test ]
        public void TestUpdateResult()
        {
            var result1 = new UpdateResult( 39345, 432345 );
            var result2 = new UpdateResult( 34594, 3434444 );
            Console.WriteLine( result1.GetDifferencePercent( result2 ) );
        }
        
        [ Test ]
        public void CategoryEnd()
        {
            var categoryIds = new [] { 10112000, 10101020, 20803000, 10200000, 10000000 };
            var ends = categoryIds.Select( id => CategoryHelper.GetEndCategory( id ) ).ToList();
        }
        
        [ Test ]
        public void CompareStrings()
        {
            var string1 = "https://assets.adidas.com/images/w_1080,h_1080,f_auto,q_auto:sensitive,fl_lossy/"; //work
            var string2 = "https://assets.adidas.com/images/w_1080,h_1080,f_auto,q_auto:sensitive,fl_lossy/"; //--
            Assert.AreEqual( string1, string2 );
        }

        [ Test ]
        public void TestLoggerSendRemote()
        {
            LogWriter.Log( "test improve 1", true );
            LogWriter.Log( "test not improve", false );
            LogWriter.Log( "test improve 2", true );
            var messenger = CreateMessenger();
            LogWriter.WriteLog( messenger.Send );
        }

        [ Test ]
        public void TestTelegram()
        {
            var messenger = CreateMessenger();
            messenger.Send( "Test" );
        }

        [ Test ]
        public void TestPathGenerator() {
            var path = FilePathHelper.CombinePath( "o:/test/", "old/" );
            ;
        }
        
        [ Test ]
        public void UrlParserTest()
        {
            var repository = new TheStoreRepository( "server=185.221.152.127;user=thestore;password=moonlike-mitts-0Concord;database=theStore;", "10.3.27" );
            var worker = new CompareWorker( repository, new BackgroundBaseContext("1","1"));
            var result = worker.Convert( new UrlInfo(
                1,
                "https://thestore.ru/brand-jb4/",
                "https://thestore.matveyko.su/brand-jb4/" ) );
        }

        [ Test ]
        public void FindDuplicates()
        {
            var indexIdPath = @"o:\admitad\workData\scrollApi\products-1.txt";
            var indexOldIdPath = @"o:\admitad\workData\scrollApi\products-old-1.txt";
            var duplicate = @"o:\admitad\workData\scrollApi\duplicate.txt";

            var newIds = File.ReadAllLines( indexIdPath ).ToList();
            var oldIds = File.ReadAllLines( indexOldIdPath ).ToList();
            var newIdsSet = newIds.ToHashSet();
            var oldIdsSet = oldIds.ToHashSet();
            var duplicates = oldIdsSet.Where( id => newIdsSet.Contains( id ) ).ToHashSet();
            File.WriteAllLines( duplicate, duplicates );

        }
        
        private IMessenger CreateMessenger()
        {
            var messengerSettings = new MessengerSettings();
            messengerSettings.Clients.Add( _telegramSettings );
            return new Messenger.Messenger( messengerSettings );
        }
    }
}