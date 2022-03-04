// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Common.Entities;
using Common.Settings;

using Newtonsoft.Json;

namespace Common.Workers
{
    public sealed class SettingsBuilder
    {

        private List<SettingsOption> _options;

        public SettingsBuilder( ISettingsRepository repository )
        {
            FillSettings( repository );
        }

        public static ApiClientSettings GetApiClientSettings()
        {
            const string settingsPath = @"o:\admitad\workData\settings.json";
            return JsonConvert.DeserializeObject<ApiClientSettings>( File.ReadAllText( settingsPath ) );
        }

        public static DbSettings GetDbSettings( string path = null)
        {
            const string settingsPath = "dbSettings.json";
            return JsonConvert.DeserializeObject<DbSettings>( File.ReadAllText( path == null ? settingsPath : $"{path}/{settingsPath}" ) );
        }
        
        public MessengerSettings GetMessengerSettings()
        {
            var settings = GetSettings();

            var messengerSettings = new MessengerSettings();
            foreach( var client in new IClientSettings[] { settings.TelegramSettings } ) {
                messengerSettings.Clients.Add( client );
            }

            return messengerSettings;
        } 
        
        public ProcessorSettings GetSettings()
        {
            var admitadSettings = new AdmitadApiSettings {
                TokensUrl = GetString("AdmitadTokensUrl"),
                ClientId = GetString("AdmitadClientId"),
                ClientSecret = GetString("AdmitadClientSecret"),
                Base64Header = GetString("AdmitadBase64Header")
            };
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
                TelegramSettings = telegramSetting,
                AdmitadSettings = admitadSettings,
                CtrCalculationType = GetString("RatingCalculationType"),
                UrlStatisticsDebuggingEnable = GetBool("UrlStatisticsDebuggingEnable")
            };
            return settings;
        }

        private string GetOptionByName( string name ) {
            try {
                var option = _options.First( o => o.Option == name );
                return option.Value;
            }
            catch( Exception ) {
                throw new ArgumentOutOfRangeException( $"Unknown option {name}" );
            }
        }

        private int GetInt( string name ) {
            var value = GetOptionByName( name );
            return int.Parse( value );
        }

        private bool GetBool( string name ) {
            var value = GetOptionByName( name );
            return bool.Parse( value );
        }

        private string GetString( string name ) =>
            GetOptionByName( name );

        private void FillSettings( ISettingsRepository repository )
        {
            if( _options == null ||
                _options.Any() == false ) {
                _options = repository.GetSettingsOptions();
            }
        }
    }
}