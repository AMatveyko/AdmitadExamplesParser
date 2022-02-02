// a.snegovoy@gmail.com

using Common.Elastic.Workers;

namespace TheStore.Api.Front.Entity
{
    public sealed class UrlStatisticsControllerRequirements
    {
        public UrlStatisticsIndexClient IndexClient { get; set; }
        public bool IsDebug { get; set; }
    }
}