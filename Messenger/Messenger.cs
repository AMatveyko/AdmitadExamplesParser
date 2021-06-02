// a.snegovoy@gmail.com

using Common.Settings;

namespace Messenger
{
    public class Messenger : IMessenger
    {

        private readonly MessengerSettings _settings;

        public Messenger( MessengerSettings settings ) {
            _settings = settings;
        }
            
        public void Send( string message )
        {
            var clients = ClientBuilder.GetMessengers( _settings.Clients );
            foreach( var client in clients ) {
                client.Send( message );
            }
        }
    }
}