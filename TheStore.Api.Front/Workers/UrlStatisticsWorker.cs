// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Common.Elastic.Workers;
using Common.Entities;
using Common.Helpers;

using TheStore.Api.Front.Entity;

namespace TheStore.Api.Front.Workers
{

    internal sealed class UrlStatisticsWorker
    {

        private const string Yandex = "yandex";
        private const string Google = "google";
        
        private const int DaysPassed = 30;
        private const int Size = 5;

        private const string YandexVisitField = "dateLastShowYandex";
        private const string GoogleVisitField = "dateLastShowGoogle";
        private const string OtherVisitField = "other";
        
        private static readonly Regex UrlPattern = new Regex(
            @"https:\/\/(?<domain>((\w+\.)?thestore\.ru))\/dlya\/(?<vertical>(\w+))\/(.+)",
            RegexOptions.Compiled);
        
        
        private readonly UrlStatisticsIndexClient _client;

        public UrlStatisticsWorker( UrlStatisticsIndexClient client ) => _client = client;

        public void Update( UrlStatisticsParameters parameters )
        {
            var entry = GetEntry( parameters.Url );
            if( entry == null ) {
                CreateAndInsert( parameters );
            }
            else {
                UpdateEntry( entry, parameters );
            }
        }

        public void AddUrls( List<string> urls )
        {
            var entries = urls.Select( u => new UrlStatisticEntry( u ) ).ToList();
            _client.Insert( entries );
        }

        public List<UrlStatisticEntry> GetUrls( BotType botType, string url )
        {
            var (domain, vertical) = GetDomainAndVerticalFromUrl( url );
            var fieldName = GetFieldName( botType );
            var dateThreshold = GetDateThreshold();

            return _client.GetUrlsInfos( domain, vertical, fieldName, dateThreshold, Size );
        }

        private void CreateAndInsert( UrlStatisticsParameters parameters )
        {
            var entry = new UrlStatisticEntry( parameters.Url );
            SetData( entry, parameters );
            _client.Insert( entry );
        }

        private static void SetData( UrlStatisticEntry entry, UrlStatisticsParameters parameters )
        {
            switch( parameters.BotType ) {
                case BotType.Yandex:
                    SetYandex( entry, parameters.ErrorCode );
                    break;
                case BotType.Google:
                    SetGoogle( entry, parameters.ErrorCode );
                    break;
                case BotType.Other:
                    break;
                case BotType.User:
                    SetNonSearchBot( entry, parameters.ErrorCode );
                    SetIndexInfo( entry, parameters.Referer );
                    break;
                default:
                    throw new ArgumentException( "bot type notFound" );
            }
        }
        
        private static void SetYandex( IUrlStatisticsEntryYandex entry, short errorCode )
        {
            entry.NumberVisitsYandex = entry.NumberVisitsYandex == null ? 1 : entry.NumberVisitsYandex + 1;
            entry.LastErrorCodeYandex = errorCode;
            entry.LastVisitDateYandex = DateTime.Now;
        }

        private static void SetGoogle( IUrlStatisticsEntryGoogle entry, short errorCode )
        {
            entry.NumberVisitsGoogle = entry.NumberVisitsGoogle == null ? 1 : entry.NumberVisitsGoogle + 1;
            entry.LastErrorCodeGoogle = errorCode;
            entry.LastVisitDateGoogle = DateTime.Now;
        }

        private static void SetNonSearchBot( IUrlStatisticsEntryNonBot entry, short errorCode ) {
            entry.NumberVisitsNonBot = entry.NumberVisitsNonBot == null ? 1 : entry.NumberVisitsNonBot + 1;
            entry.LastErrorCodeNonBot = errorCode;
        }
            

        private static void SetIndexInfo( UrlStatisticEntry entry, string referer )
        {
            if( string.IsNullOrWhiteSpace( referer ) ) {
                return;
            }

            switch( DetermineSearchEngine( referer ) ) {
                case Yandex:
                    SetYandexIndexInfo( entry );
                    break;
                case Google:
                    SetGoogleIndexInfo( entry );
                    break;
                default:
                    throw new ArgumentException( "search engine notFound" );
            }
        }

        private static void SetYandexIndexInfo( IUrlStatisticsEntryIndexedYandex entry )
        {
            entry.IndexedYandex = true;
            entry.DateLastIndexCheckYandex = DateTime.Now;
        }

        private static void SetGoogleIndexInfo(
            IUrlStatisticsEntryIndexedGoogle entry )
        {
            entry.IndexedGoogle = true;
            entry.DateLastIndexCheckGoogle = DateTime.Now;
        }
        
        private void UpdateEntry( UrlStatisticEntry entry, UrlStatisticsParameters parameters )
        {
            SetData( entry, parameters );
            UpdateEntry( entry );
        }

        public void UpdateEntry( UrlStatisticEntry entry ) => _client.Update( entry );
        
        private UrlStatisticEntry GetEntry( string url )
        {
            var id = HashHelper.GetMd5Hash( url );
            return _client.Get( id );
        }

        private static string DetermineSearchEngine(
            string referer ) =>
            referer switch {
                Yandex => Yandex,
                Google => Google
            };
        
        private static (string, string) GetDomainAndVerticalFromUrl( string url )
        {
            var match = UrlPattern.Match( url );
            if( match.Success == false ) {
                throw new ArgumentException( $"{url} not suitable url" );
            }

            return ( match.Groups[ "domain" ].Value, match.Groups[ "vertical" ].Value );
        }

        private static string GetFieldName(
            BotType botType ) =>
            botType switch {
                BotType.Yandex => YandexVisitField,
                BotType.Google => GoogleVisitField,
                _ => OtherVisitField
            };

        private static DateTime GetDateThreshold() => DateTime.Now.AddDays( -DaysPassed );

    }
}