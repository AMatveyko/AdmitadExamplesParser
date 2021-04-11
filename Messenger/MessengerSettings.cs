// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Messenger
{
    public class MessengerSettings
    {
        public List<IClientSettings> Clients { get; set; } = new();
    }
}