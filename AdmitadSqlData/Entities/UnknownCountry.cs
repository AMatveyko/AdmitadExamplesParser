// a.snegovoy@gmail.com

using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace AdmitadSqlData.Entities
{
    [ Table( "unknown_countries" ) ]
    public sealed class UnknownCountry
    {
        [ Column( "id" ) ]
        public int Id { get; set; }
        [ Column( "name" ) ]
        public string Name { get; set; }
        [ Column( "offer_count" ) ]
        public int OfferCount { get; set; }
    }
}