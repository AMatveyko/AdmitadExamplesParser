// a.snegovoy@gmail.com

using System;

namespace AdmitadApi.Entities
{
    internal sealed class AuthorizationData
    {
        private DateTime _lastAuthorizationTime;
        private int _lifeTime;
        
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public bool IsNeedAuthorization() => IsTokensEmpty() || IsExpired();
        public bool IsTokensEmpty() => string.IsNullOrEmpty( AccessToken ) && string.IsNullOrEmpty( RefreshToken );
        public void SetAuthorizationTime() => _lastAuthorizationTime = DateTime.Now;
        public bool IsExpired() => _lastAuthorizationTime <= DateTime.Now.AddSeconds( -_lifeTime );
        public void SetAuthorizationData(
            string accessToken,
            string refreshToken,
            int lifeTime ) =>
            ( AccessToken, RefreshToken, _lifeTime ) = ( accessToken, refreshToken, lifeTime );
    }
}