// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Common.Elastic.Workers;
using Common.Entities;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Entity;
using TheStore.Api.Front.Workers;

namespace TheStore.Api.Front.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class UrlStatisticsController : ControllerBase
    {

        private static readonly Regex UrlPattern = new Regex( @"^https:\/\/.*", RegexOptions.Compiled );
        private readonly UrlStatisticsIndexClient _indexClient;

        public UrlStatisticsController(
            UrlStatisticsIndexClient client ) =>
            _indexClient = client;
        
        [ HttpGet ]
        [ Route( "Update" ) ]
        public IActionResult Update( string url, string botType, short? errorCode, string referer )
        {
            DetermineUrl( url );
            var determinedBotType = DetermineBotType( botType );
            var parameters = new UrlStatisticsParameters( url, determinedBotType, errorCode, referer );
            var result = GetWorker().Update( parameters );

            return new ObjectResult( result );
        }

        [ HttpPost ]
        [ Route( "AddUrl" ) ]
        public void AddUrl( string url ) => AddUrls( new List<string> { url } );

        [ HttpPost ]
        [ Route( "AddUrls" ) ]
        public void AddUrls( List<string> urls ) {
            DetermineUrls( urls );
            GetWorker().AddUrls( urls );
        }
            

        private static BotType DetermineBotType(
            string typeName ) =>
            typeName.ToLower() switch {
                "yandex" => BotType.Yandex,
                "google" => BotType.Google,
                "notbot" => BotType.NotBot,
                _ => throw new ArgumentException( "bot type notfound" )
            };
        
        private IUrlStatisticsWorker GetWorker() =>
            new UrlStatisticsWithQueues( _indexClient );

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