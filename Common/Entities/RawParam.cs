// a.snegovoy@gmail.com

using System.Xml.Serialization;

namespace Common.Entities
{
    public sealed class RawParam
    {
        [ XmlAttribute( "name" ) ] public string NameFromXml { get; set; }
        [ XmlAttribute( "unit" ) ] public string UnitFromXml { get; set; }
        [ XmlText ] public string ValueFromXml { get; set; }


        [ XmlIgnore ] public string Name => GetToLower( NameFromXml );
        [ XmlIgnore ] internal string Unit => GetToLower( UnitFromXml );
        [ XmlIgnore ] public string Value => GetToLower( ValueFromXml );

        private static string GetToLower( string value ) =>
            value?.ToLower();
    }
}