// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Common.Elastic.Workers;
using Common.Entities;
using Common.Helpers;

using Microsoft.AspNetCore.Mvc;

using NLog;

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
        private readonly bool _isDebug;
        private static readonly Logger Statistics = LogManager.GetLogger( "UrlStatisticsForIndexLogger" );

        public UrlStatisticsController( UrlStatisticsControllerRequirements requirements) =>
            ( _indexClient, _isDebug ) = ( requirements.IndexClient, requirements.IsDebug );

        [ HttpGet ]
        [ Route( "Update" ) ]
        public IActionResult Update(
            string url,
            string botType,
            short? errorCode,
            string referer,
            short urlNumber = 5 ) =>
            ErrorHandling( () => DoUpdate( url, botType, errorCode, urlNumber, referer ) );

        private List<string> DoUpdate( string rawUrl, string botType, short? errorCode, short urlNumber, string referer )
        {
            var url = rawUrl.ToLower();
            DebugIfNeed( url, botType, errorCode, referer );
            DetermineUrl( url );
            var determinedBotType = DetermineBotType( botType );
            var parameters = new UrlStatisticsParameters( url, determinedBotType, errorCode, urlNumber, referer );
            return GetWorker().Update( parameters );
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
                "other" => BotType.Other ,
                "user" => BotType.User,
                _ => throw new ArgumentException( "bot type notfound" )
            };

        // private static BotType DetermineBotType( string typeName ) =>
        //     EnumHelper<BotType>.GetValueByName( typeName.ToLower() );

        private IUrlStatisticsWorker GetWorker() =>
            new UrlStatisticsWithQueues( _indexClient );

        private void DebugIfNeed(
            string url,
            string botType,
            short? errorCode,
            string referer )
        {
            if( _isDebug == false ) {
                return;
            }
            
            Statistics.Info( $"url: {url}\nbotType: {botType}, errorCode: { (errorCode == null ? "null" : errorCode.ToString() ) }, referer: {referer}");
        }
            
        
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

            throw new ArgumentException( $"{url} not url" );
        }

        private IActionResult ErrorHandling( Func<object> action ) {
            try {
                return new ObjectResult( new {
                    IsError = false,
                    Result = action()
                });
            }
            catch( Exception e ) {
                return new ObjectResult(
                    new {
                        IsError = true,
                        Error = e.Message
                    } );
            }
        }
    }
}