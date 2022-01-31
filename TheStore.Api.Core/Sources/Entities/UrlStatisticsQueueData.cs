// a.snegovoy@gmail.com

using Common.Entities;

namespace TheStore.Api.Core.Sources.Entities
{
    internal sealed class UrlStatisticsQueueData
    {
        public string Url { get; set; }
        public BotType BotType { get; set; }
        public short? ErrorCode { get; set; }
    }
}