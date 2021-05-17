// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdmitadSqlData.Entities
{
    [ Table( "unknown_countries" ) ]
    public sealed class UnknownCountry
    {
        [ Key ]
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "offer_count" ) ]
        public int OfferCount { get; set; }
    }
}