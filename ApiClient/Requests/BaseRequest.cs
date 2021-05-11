// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AdmitadCommon.Extensions;

using Newtonsoft.Json;

using NLog;

using RestSharp;

namespace ApiClient.Requests
{
    internal abstract class BaseRequest<T>
    {
        
        private static readonly Logger Logger = LogManager.GetLogger( "ErrorLogger" );
        
        private const string Host = "http://localhost:8080";
        private readonly string _controller;
        private readonly string _methodName;
        private List<( string, string)> _parameters;

        protected BaseRequest( string controller, string methodName )
        {
            _controller = controller;
            _methodName = methodName;
        }

        protected void AddParam( string name, string value )
        {
            if( name.IsNullOrWhiteSpace() ||
                value.IsNullOrWhiteSpace() ) {
                throw new Exception( "Empty parameter" );
            }

            _parameters ??= new();
            _parameters.Add( ( name, value ) );
        }

        public T Execute()
        {
            const string template = "{0}/{1}/{2}";
            var url = string.Format( template, Host, _controller, _methodName );
            if( _parameters != null &&
                _parameters.Any() ) {
                var @params = 
                    string.Join( 
                        "&", 
                        _parameters.Select( p => $"{p.Item1}={ HttpUtility.UrlEncode(  p.Item2 ) }" ) );
                url += $"?{ @params }";
            }

            return DoExecute( new Uri( url ) );
        }
        
        private static T DoExecute( Uri uri )
        {
            try {
                var response = GetResponse( uri );
                return GetContent( response.Content );
            }
            catch( Exception e ) {
                Logger.Error( e );
                throw new Exception( "Исключение!" );
            }
        }
        
        private static T GetContent( string content ) =>
            JsonConvert.DeserializeObject<T>( content );
        
        private static IRestResponse GetResponse( Uri uri )
        {
            var client = new RestClient( $"{uri.Scheme}://{uri.Host}:{uri.Port}" );
            var request = new RestRequest( uri.PathAndQuery, DataFormat.Json);
            return client.Get(request);
        }
    }
}