// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AdmitadCommon.Entities.Api;

using NLog;

using TheStore.Api.Core.Sources.Entities;
using TheStore.Api.Front.Data.Entities;
using TheStore.Api.Front.Data.Repositories;

namespace TheStore.Api.Core.Sources.Workers
{
    public sealed class CompareWorker
    {

        private static readonly Logger LoggerError = LogManager.GetLogger( "DownloadPageError" );
        private static readonly Logger LoggerInfo = LogManager.GetLogger( "DownloadPageInfo" );
        
        private readonly TheStoreRepository _repository;
        private readonly BackgroundBaseContext _context;

        private static Regex _countPattern = new ( @"<code>(?<products>(\d+))\|(?<shops>(\d+))?<\/code>", RegexOptions.Compiled );
        private static Regex _emptyPagePattern =
            new(@"К сожалению (.*), но обратите внимание на популярное сегодня", RegexOptions.Compiled);

        public CompareWorker( TheStoreRepository repository, BackgroundBaseContext context ) =>
            ( _repository, _context ) = ( repository, context );
        
        public void CompareAndWrite( List<UrlInfo> infos )
        {
            DoCompareAndWrite( infos );
        }

        private void DoCompareAndWrite( List<UrlInfo> infos )
        {
            var compareList = infos
                .AsParallel()
                .Select( Convert ).ToList();
            _context.AddMessage( "Собрали количества товаров" );
            _repository.UpdateCompareList( compareList );
            _context.AddMessage( "Записали в бд" );
        }

        public CompareListingDb Convert( UrlInfo info )
        {
            var oldSitePageTask = Task.Run( () => DownloadAndParseAsync( info.OldSiteUrl, true ) );
            var newSitePageTask = Task.Run( () => DownloadAndParseAsync( info.NewSiteUrl, false ) );

            Task.WaitAll( oldSitePageTask, newSitePageTask );

            var oldSitePage = oldSitePageTask.Result;
            var newSitePage = newSitePageTask.Result;

            _context.CalculatePercent();
            
            return new CompareListingDb {
                AddDate = DateTime.Now,
                Url = info.NewSiteUrl,
                Visits = info.Visits,
                OldSiteProductCount = oldSitePage.ProductsCount,
                OldSiteShopCount = oldSitePage.ShopCount,
                NewSiteProductCount = newSitePage.ProductsCount,
                NewSiteShopCount = newSitePage.ShopCount
            };
        }

        private async Task<PageInfo> DownloadAndParseAsync( string url, bool isOld )
        {
            var data = await DownloadPageAsync( url, isOld );
            return Parse( data );
        }
        
        private PageInfo Parse( string data )
        {
            var emptyMatch = _emptyPagePattern.Match( data );
            if( emptyMatch.Success ) {
                return new PageInfo( 0, 0 );
            }
            
            var m = _countPattern.Match( data );
            if( m.Success == false ) {
                return new PageInfo( -1, -1 );
            }

            int.TryParse( m.Groups[ "products" ].Value, out var productsCount );
            int.TryParse( m.Groups[ "shops" ].Value, out var shopsCount );

            return new PageInfo( productsCount, shopsCount );
        }
        
        private static async Task<string> DownloadPageAsync( string url, bool isOld )
        {
            var handler = new HttpClientHandler();
            using var httpClient = new HttpClient();
            if( isOld ) {
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add( new Cookie( "is_admin", "1" ) { Domain = "thestore.ru" } );
            }

            try {
                var response = await httpClient.GetAsync( url );
                LoggerInfo.Info( $"{response.StatusCode} {url}" );
                return await response.Content.ReadAsStringAsync();
            }
            catch( Exception e ) {
                LoggerInfo.Error( e, url );
                return string.Empty;
            }
        }
    }
}