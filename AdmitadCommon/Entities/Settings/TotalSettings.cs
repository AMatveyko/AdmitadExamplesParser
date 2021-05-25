// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace AdmitadCommon.Entities.Settings
{
    public sealed class TotalSettings
    {
        [ JsonProperty( "mysql_host" ) ]
        public string MySQLHost { get; set; }
        [ JsonProperty( "mysql_user" ) ]
        public string MySQLUser { get; set; }
        [ JsonProperty( "mysql_password" ) ]
        public string MySQLPassword { get; set; }
        [ JsonProperty( "mysql_database" ) ]
        public string MySQLDatabase { get; set; }
        [ JsonProperty( "mysql_listingCashTable" ) ]
        public string MySQLListingCashTable { get; set; }
    }
}