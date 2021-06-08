// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

using Newtonsoft.Json;

using NLog;

using RestSharp;

namespace Web.Common.Entities.Requests
{
    public abstract class BaseRequest<T>
    {
        
        private readonly string _controller;
        private readonly string _methodName;
        private List<( string, string)> _parameters;

        private RequestSettings _settings;

        protected BaseRequest( string controller, string methodName, RequestSettings settings )
        {
            ( _controller, _methodName, _settings ) = ( controller, methodName, settings );
        }

        protected void AddParam( string name, string value )
        {
            if( string.IsNullOrWhiteSpace( name ) ||
                string.IsNullOrWhiteSpace( value ) ) {
                throw new Exception( "Empty parameter" );
            }

            _parameters ??= new List<(string, string)>();
            _parameters.Add( ( name, value ) );
        }

        public T Execute()
        {
            const string template = "{0}/{1}/{2}";
            var url = string.Format( template, _settings.Host, _controller, _methodName );
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
        
        private T DoExecute( Uri uri )
        {
            try {
                var response = GetResponse( uri );
                if( response.StatusCode != HttpStatusCode.OK ) {
                    _settings.Logger?.Error( $"{uri}: { response.StatusCode }" );
                }
                return GetContent( response.Content );
            }
            catch( Exception e ) {
                _settings.Logger?.Error( e );
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