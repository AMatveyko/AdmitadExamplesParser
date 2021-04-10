// a.snegovoy@gmail.com

using System.Xml.Serialization;

namespace AdmitadExamplesParser.Entities
{
    public sealed class AllFile
    {
        [XmlElement("yml_catalog")] public YmlCatalog YmlCatalog { get; set; }
    }
}