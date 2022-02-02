// a.snegovoy@gmail.com

using Common.Entities;

namespace TheStore.Api.Front.Entity
{
    internal class UrlStatisticsParameters
    {

        public UrlStatisticsParameters(
            string url,
            BotType botType,
            short? errorCode,
            string referer ) =>
            ( Url, BotType, ErrorCode, Referer ) = ( url, botType, errorCode ?? 200, referer );
        
        public string Url { get; }
        public BotType BotType { get; }
        public short ErrorCode { get; }
        public string Referer { get; }
    }
}