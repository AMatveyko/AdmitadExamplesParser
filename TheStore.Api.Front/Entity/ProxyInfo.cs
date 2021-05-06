// a.snegovoy@gmail.com

namespace TheStore.Api.Front.Entity
{
    internal sealed class ProxyInfo
    {

        public ProxyInfo( string url, string userName, string password )
        {
            Url = url;
            UserName = userName;
            Password = password;
        }

        public ProxyInfo( string url )
        {
            Url = url;
        }
        
        public string Url { get; }
        public string UserName { get; }
        public string Password { get; }
    }
}