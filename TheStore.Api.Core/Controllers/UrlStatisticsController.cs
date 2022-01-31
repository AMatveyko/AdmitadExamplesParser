// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Common.Elastic.Workers;
using Common.Entities;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Core.Sources.Workers;

namespace TheStore.Api.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class UrlStatisticsController : ControllerBase
    {

        private static readonly Regex UrlPattern = new ( @"^https:\/\/.*", RegexOptions.Compiled );
        private readonly UrlStatisticsIndexClient _indexClient;

        public UrlStatisticsController(
            UrlStatisticsIndexClient client ) =>
            _indexClient = client;
        
        [ HttpGet ]
        [ Route( "Update" ) ]
        public void Update( string url, string botType, short? errorCode )
        {
            DetermineUrl( url );
            var determinedBotType = DetermineBotType( botType );
            GetWorker( false ).Update( url,determinedBotType, errorCode );
        }

        [ HttpPost ]
        [ Route( "AddUrl" ) ]
        public void AddUrl( string url ) => AddUrls( new List<string> { url } );

        [ HttpPost ]
        [ Route( "AddUrls" ) ]
        public void AddUrls( List<string> urls ) {
            DetermineUrls( urls );
            GetWorker(false).AddUrls( urls );
        }
            

        private static BotType DetermineBotType(
            string typeName ) =>
            typeName.ToLower() switch {
                "yandex" => BotType.Yandex,
                "google" => BotType.Google,
                _ => throw new ArgumentException( "bot type notfound" )
            };
        
        private IUrlStatisticsWorker GetWorker( bool withQueues ) =>
            withQueues ? new UrlStatisticsWithQueues( _indexClient ) : new UrlStatisticsWorker( _indexClient );

        private static void DetermineUrls( List<string> urls ) {
            foreach( var url in urls ) {
                DetermineUrl( url );
            }
        }
        
        private static void DetermineUrl(
            string url )
        {
            if( UrlPattern.IsMatch( url ) ) {
                return;
            }

            throw new ArgumentException( $"{url}  not url" );
        }
    }
}