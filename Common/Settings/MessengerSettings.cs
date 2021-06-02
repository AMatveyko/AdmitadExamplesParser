// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace Common.Settings
{
    public sealed class MessengerSettings
    {
        public List<IClientSettings> Clients { get; set; } = new List<IClientSettings>();
    }
}