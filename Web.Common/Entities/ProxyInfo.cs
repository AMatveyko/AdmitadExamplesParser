// a.snegovoy@gmail.com

namespace Web.Common.Entities
{
    public sealed class ProxyInfo
    {

        public ProxyInfo( string ip, string port, string userName, string password ) : this( ip, port )
        {
            UserName = userName;
            Password = password;
        }

        public ProxyInfo( string ip, string port )
        {
            Url = $"http://{ip}:{port}";
        }
        
        public string Url { get; }
        public string UserName { get; }
        public string Password { get; }
    }
}