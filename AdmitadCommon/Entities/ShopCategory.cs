// a.snegovoy@gmail.com

using System.Xml.Serialization;

namespace AdmitadCommon.Entities
{
    public sealed class ShopCategory
    {
        [ XmlAttribute( "id" ) ]
        public string Id { get; set; }
        [ XmlAttribute( "parentId" ) ] public string ParentId { get; set; }
        [ XmlText ]
        public string Name { get; set; }
        // [ XmlIgnore ]
        // public int WomenProductsNumber { get; set; }
        // [ XmlIgnore ]
        // public int MenProductsNumber { get; set; }
        // [ XmlIgnore ]
        // public int TotalProductsNumber { get; set; }
    }
}