// a.snegovoy@gmail.com

using System.Text.Json;

namespace AdmitadApi.Workers
{
    internal static class Serializer
    {
        public static T Deserialize<T>( string data ) =>
            JsonSerializer.Deserialize<T>( data );
    }
}