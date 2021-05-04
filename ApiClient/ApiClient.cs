// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Statistics;

using Newtonsoft.Json;

using NLog;

using RestSharp;

namespace ApiClient
{
    internal static class ApiClient
    {

        private static readonly Logger Logger = LogManager.GetLogger( "ErrorLoggerTarger" );

        private const string Host = "http://localhost:8080";
        private static readonly Uri UriStartIndex = new ( $"{Host}/Index/IndexAllShops?clean=false" );
        //private static readonly Uri UriStartIndex = new($"{Host}/Index/IndexShop?id=96&downloadFresh=true&clean=false");
        private static readonly Uri UriShopStatistics = new ( $"{Host}/Index/GetShopStatistics" );
        private static readonly Uri UriTotalStatistics = new ( $"{Host}/Index/GetStatistics" );

        public static TopContext RunAndCheckIndex()
        {
            return Execute<TopContext>( UriStartIndex );
        }

        public static TotalShopsStatistics GetShopStatistics()
        {
            return Execute<TotalShopsStatistics>( UriShopStatistics );
        }

        public static TotalStatistics GetTotalStatistics()
        {
            return Execute<TotalStatistics>( UriTotalStatistics );
        }

        private static T Execute<T>( Uri uri )
        {
            try {
                var response = GetResponse( uri );
                return GetContent<T>( response.Content );
            }
            catch( Exception e ) {
                Logger.Error( e );
                throw new Exception( "Исключение!" );
            }
        }
        
        private static T GetContent<T>( string content ) =>
            JsonConvert.DeserializeObject<T>( content );
        
        private static IRestResponse GetResponse( Uri uri )
        {
            var client = new RestClient( $"{uri.Scheme}://{uri.Host}:{uri.Port}" );
            var request = new RestRequest( uri.PathAndQuery, DataFormat.Json);
            return client.Get(request);
        }
    }
}