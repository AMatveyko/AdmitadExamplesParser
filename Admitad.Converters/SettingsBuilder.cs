// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Settings;

using AdmitadSqlData.Helpers;

using Messenger;

using Newtonsoft.Json;

namespace Admitad.Converters
{
    public static class SettingsBuilder
    {

        private static List<SettingsOption> _options;

        static SettingsBuilder()
        {
            FillSettings();
        }

        public static ApiClientSettings GetApiClientSettings()
        {
            const string settingsPath = @"o:\admitad\workData\settings.json";
            return JsonConvert.DeserializeObject<ApiClientSettings>( File.ReadAllText( settingsPath ) );
        }

        public static DbSettings GetDbSettings()
        {
            const string settingsPath = "dbSettings.json";
            return JsonConvert.DeserializeObject<DbSettings>( File.ReadAllText( settingsPath ) );
        }
        
        public static MessengerSettings GetMessengerSettings()
        {
            var settings = GetSettings();

            var messengerSettings = new MessengerSettings();
            foreach( var client in new IClientSettings[] { settings.TelegramSettings } ) {
                messengerSettings.Clients.Add( client );
            }

            return messengerSettings;
        } 
        
        public static ProcessorSettings GetSettings()
        {
            var telegramSetting = new TelegramSettings {
                Enabled = GetBool( "TelegramEnabled" ),
                Token = GetString( "TelegramBotToken" ),
                ChatId = GetString( "TelegramChatId" )
            };
            var elasticSearchClientSettings = new ElasticSearchClientSettings {
                ComponentForIndex = ComponentType.ElasticSearch,
                ElasticSearchUrl = GetString( "ElasticSearchClientUrl" ),
                DefaultIndex = GetString( "ElasticSearchClientIndex" ),
                FrameSize = GetInt( "ElasticSearchClientFrameSize" ),
                ShopName = GetString( "ElasticSearchClientShopName" )
            };
            var settings = new ProcessorSettings {
                AttemptsToDownload = GetInt( "AttemptsToDownload" ),
                EnableExtendedStatistics = GetBool( "EnableExtendedStatistics" ),
                DirectoryPath = GetString( "DirectoryPath" ),
                DuplicateFile = GetString( "DuplicateFile" ),
                ShowStatistics = GetBool( "ShowStatistics" ),
                ElasticSearchClientSettings = elasticSearchClientSettings,
                TelegramSettings = telegramSetting
            };
            return settings;
        }

        private static string GetOptionByName( string name ) {
            try {
                var option = _options.First( o => o.Option == name );
                return option.Value;
            }
            catch( Exception ) {
                throw new ArgumentOutOfRangeException( $"Unknown option {name}" );
            }
        }

        private static int GetInt( string name ) {
            var value = GetOptionByName( name );
            return int.Parse( value );
        }

        private static bool GetBool( string name ) {
            var value = GetOptionByName( name );
            return bool.Parse( value );
        }

        private static string GetString( string name ) =>
            GetOptionByName( name );

        private static void FillSettings()
        {
            if( _options == null ||
                _options.Any() == false ) {
                _options = new DbHelper( GetDbSettings() ).GetSettingsOptions();
            }
        }
    }
}