// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AdmitadCommon.Entities;

using Messenger.Clients;

namespace Messenger
{
    internal static class ClientBuilder
    {

        public static IEnumerable<IMessenger> GetMessengers( IEnumerable<IClientSettings> settings ) =>
            settings.Where( s => s.Enabled ).Select( CreateClient );

        private static IMessenger CreateClient(
            IClientSettings settings )
        {
            return settings.Type switch {
                MessengerType.Telegram => new Telegram( settings ),
                _ => throw new ArgumentException( "Unknown messenger client tyepe" )
            };
        }
    }
}