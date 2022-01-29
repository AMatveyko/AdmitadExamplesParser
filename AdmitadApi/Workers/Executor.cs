// a.snegovoy@gmail.com

using System;
using System.IO;
using System.Net;
using System.Text;

using AdmitadApi.Entities;
using AdmitadApi.Requests;

using Common.Workers;

using TheStore.Api.Front.Data.Repositories;

namespace AdmitadApi.Workers
{
    internal sealed class Executor
    {
        
        // scopes
        // advcampaigns              - Список партнерских программ
        // advcampaigns_for_website  - Список программ для площадки
        // websites                  - Список площадок
        
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _base64Header;

        private readonly AuthorizationData _authorizationData;

        public Executor() {
            _authorizationData = new AuthorizationData();
            var dbSettings = SettingsBuilder.GetDbSettings();
            var settings = new SettingsBuilder( new TheStoreRepository( dbSettings) ).GetSettings();
            var admitadSettings = settings.AdmitadSettings;
            _url = admitadSettings.TokensUrl;
            _clientId = admitadSettings.ClientId;
            _clientSecret = admitadSettings.ClientSecret;
            _base64Header = admitadSettings.Base64Header;
        }
        
        public T Execute<T>( IApiRequest apiRequest ) {
            if( _authorizationData.IsNeedAuthorization() ) {
                Authorization();
            }

            var request = GetRequest( apiRequest.Get() );
            return Execute<T>( request );
        }

        private void Authorization()
        {
            if( _authorizationData.IsTokensEmpty() ) {
                GetTokens();
            }

            if( _authorizationData.IsExpired() ) {
                RefreshTokens();
            }
        }

        private void RefreshTokens() {
            var content =
                $"grant_type=refresh_token&client_id={_clientId}&refresh_token={_authorizationData.RefreshToken}&client_secret={_clientSecret}";
            GetTokens( content, false );
        }

        private void GetTokens() {
            var content = $"grant_type=client_credentials&client_id={_clientId}&scope=advcampaigns advcampaigns_for_website websites";
            GetTokens( content, true );
        }

        private void GetTokens( string content, bool isAuthorization ) {
            SetAuthorizationTime();
            var request = GetAuthorizationRequest( isAuthorization );
            var result = Execute<AuthorizationResponse>( request, content );
            SetAuthorizationData( result );
        }
        
        private static T Execute< T >( WebRequest request, string content ) {
            
            var byteArray = Encoding.UTF8.GetBytes(content);
            request.ContentLength = byteArray.Length;
            using var reqStream = request.GetRequestStream();
            reqStream.Write(byteArray, 0, byteArray.Length);
            return Execute<T>( request );
        }

        private static T Execute< T >( WebRequest request ) {
            using var response = request.GetResponse();
            using var respStream = response.GetResponseStream();
            using var reader = new StreamReader(respStream);
            var data = reader.ReadToEnd();
            return Serializer.Deserialize<T>( data );
        }

        private WebRequest GetRequest( string url ) {
            var request = WebRequest.Create( url );
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add( HttpRequestHeader.Authorization, $"Bearer {_authorizationData.AccessToken}");

            return request;
        }
        
        private WebRequest GetAuthorizationRequest( bool isAuthorization ) {
            var request = WebRequest.Create( _url );
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            if( isAuthorization ) {
                request.Headers.Add( HttpRequestHeader.Authorization, $"Basic {_base64Header}");
            }

            return request;
        }

        private void SetAuthorizationTime() => _authorizationData.SetAuthorizationTime();
        private void SetAuthorizationData( AuthorizationResponse response ) =>
            _authorizationData.SetAuthorizationData( response.AccessToken, response.RefreshToken, response.ExpiresIn );

    }
}