// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace Common.Settings
{
    public sealed class ApiClientSettings : DbSettings
    {
        [ JsonProperty( "mysql_listingCashTable" ) ]
        public string ListingCashTable { get; set; }
    }
}