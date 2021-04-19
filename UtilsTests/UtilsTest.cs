using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using Messenger;

using NUnit.Framework;

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

        private IMessenger CreateMessenger()
        {
            var messengerSettings = new MessengerSettings();
            messengerSettings.Clients.Add( _telegramSettings );
            return new Messenger.Messenger( messengerSettings );
        }
    }
}