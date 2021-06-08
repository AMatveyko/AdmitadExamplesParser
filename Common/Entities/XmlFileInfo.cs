// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public sealed class XmlFileInfo
    {
        public XmlFileInfo(
            string name,
            string nameLatin,
            string xmlFeed,
            int shopId,
            int weight,
            byte versionProcessing,
            DateTime? lastUpdate )
        {
            Name = name;
            NameLatin = nameLatin;
            XmlFeed = xmlFeed;
            ShopId = shopId;
            Weight = weight;
            VersionProcessing = versionProcessing;
            LastUpdate = lastUpdate.GetValueOrDefault();
        }
        
        public string Name { get; }
        public string NameLatin { get; }
        public string XmlFeed { get; }
        public int ShopId { get; }
        public int Weight { get; }
        public byte VersionProcessing { get; set; }
        public DateTime LastUpdate { get; }
    }
}