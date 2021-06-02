// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

namespace TheStore.Api.Front.Data.Entities
{
    [ Table( "proxies" ) ]
    public sealed class ProxyDb
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "host" ) ]
        public string Host { get; set; }
        [ Column("port" ) ]
        public string Port { get; set; }
        [ Column("name" ) ]
        public string User { get; set; }
        [ Column( "pwd" ) ]
        public string Password { get; set; }
    }
}