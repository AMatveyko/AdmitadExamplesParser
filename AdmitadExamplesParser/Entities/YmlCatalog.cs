// a.snegovoy@gmail.com

using System.Xml.Serialization;

namespace AdmitadExamplesParser.Entities
{
    [ XmlRoot( "yml_catalog" ) ]
    public sealed class YmlCatalog
    {
        [ XmlElement( "shop" ) ]
        public Shop Shop { get; set; }
    }
}