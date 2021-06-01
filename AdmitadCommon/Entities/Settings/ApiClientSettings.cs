// a.snegovoy@gmail.com

using Newtonsoft.Json;

namespace AdmitadCommon.Entities.Settings
{
    public sealed class ApiClientSettings : DbSettings
    {
        [ JsonProperty( "mysql_listingCashTable" ) ]
        public string ListingCashTable { get; set; }
    }
}