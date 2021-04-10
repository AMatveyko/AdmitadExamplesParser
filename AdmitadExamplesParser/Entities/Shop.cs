// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Xml.Serialization;

using AdmitadCommon.Entities;

namespace AdmitadExamplesParser.Entities
{
    public sealed class Shop
    {
        [ XmlArray( "categories" ) ]
        [ XmlArrayItem( "category" ) ]
        public List<ShopCategory> Categories { get; set; }
        [ XmlElement( "local_delivery_cost" ) ]
        public decimal? LocalDeliveryCost { get; set; }
        [ XmlElement( "offers" ) ]
        public List<Offer> Offers { get; set; }
    }
}