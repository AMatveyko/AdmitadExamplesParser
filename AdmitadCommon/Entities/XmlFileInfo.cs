// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public sealed class XmlFileInfo
    {
        public XmlFileInfo(
            string name,
            string nameLatin,
            string xmlFeed,
            int shopId )
        {
            Name = name;
            NameLatin = nameLatin;
            XmlFeed = xmlFeed;
            ShopId = shopId;
        }
        
        public string Name { get; }
        public string NameLatin { get; }
        public string XmlFeed { get; }
        public int ShopId { get; }
    }
}