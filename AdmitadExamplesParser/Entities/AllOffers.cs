// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using AdmitadCommon.Entities;

namespace AdmitadExamplesParser.Entities
{
    [ Serializable ]
    [ XmlRoot( "offers" ) ]
    public sealed class AllOffers
    {
        [ XmlElement( "Offer" ) ]
        private List<Offer> Offers { get; set; } 
    }
}