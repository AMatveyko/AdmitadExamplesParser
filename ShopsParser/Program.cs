using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;
using Common.Workers;

using ShopsParser.Parsers;

using TheStore.Api.Front.Data.Repositories;

using Web.Common.Entities;
using Web.Common.Workers;

namespace ShopsParser
{

    class Program
    {
        
        private static string Path = @"o:\admitad\tests\";
        
        static void Main( string[] args ) {
            const int shopId = 113;
            ParseCategory( shopId );
        }

        private static void ParseCategory(
            int shopId )
        {
            var repository = new TheStoreRepository( SettingsBuilder.GetDbSettings() );
            var shopInfo = repository.GetShopById( shopId );
            var shopCategories = repository.GetShopCategories( shopId );
            var elasticSettings = new SettingsBuilder( repository ).GetSettings().ElasticSearchClientSettings;
            var elasticClient = IndexClient.CreateIndexClient( elasticSettings, new BackgroundBaseContext( "1", "1" ) );
            var categoryAndProduct = shopCategories
                .Select(
                    c => ( c.CategoryId,
                        elasticClient.GetFirstEnableProductByShopIdAndCategoryId( shopId.ToString(), c.CategoryId ) ) )
                .Where( c => c.Item2 != null ).Select( i => ( i.CategoryId, GetUrl( i.Item2.Url )) ).ToList();
            var dataList = categoryAndProduct.Select( i  => 
                ( i.CategoryId, Parse( shopInfo.LatinName, i.Item2, i.CategoryId ) ) )
                .Select( i => ( i.CategoryId, i.Item2.Item1, i.Item2.Item2 ) ).ToList();
            repository.UpdateCategoryAgeAndGender( dataList, shopInfo.Id );
        }

        private static (Age, Gender) Parse( string shopName, string url, string categoryId )
        {
            var filePath = $"{Path}{shopName}-{categoryId}";
            
            string data;
            if( File.Exists( filePath ) == false ) {
                var dataTask = WebRequester.RequestString( url, new ProxyInfo( "10.2.13.1", "3128" ) );
                Task.WaitAll( dataTask );
                data = dataTask.Result;
                File.WriteAllText( filePath, data );
            }
            else {
                data = File.ReadAllText( filePath );
            }

            var parser = GetParser( shopName );
            return parser.Parse( data );

        }

        private static IParser GetParser( string shopName ) =>
            shopName switch {
                "vipavenue" => new VipAvenuCategoryParser(),
                _ => throw new ArgumentException()
            };

        private static string GetUrl( string rawUrl )
        {
            rawUrl = rawUrl.Split( "ulp=" )[ 1 ];
            return HttpUtility.UrlDecode( rawUrl );
        }
    }
}