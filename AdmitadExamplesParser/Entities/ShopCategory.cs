// a.snegovoy@gmail.com

using System.Xml.Serialization;

namespace AdmitadExamplesParser.Entities
{
    public sealed class ShopCategory
    {
        [ XmlAttribute( "id" ) ]
        public string Id { get; set; }
        [ XmlAttribute( "parentId" ) ] public string ParentId { get; set; }
        [ XmlText ]
        public string Name { get; set; }
    }
}