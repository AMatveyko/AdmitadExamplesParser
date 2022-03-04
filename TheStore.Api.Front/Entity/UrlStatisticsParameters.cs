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
            short urlNumber,
            string referer ) =>
            ( Url, BotType, ErrorCode, UrlNumber, Referer ) = ( url, botType, errorCode ?? 200, urlNumber, referer );
        
        public string Url { get; }
        public BotType BotType { get; }
        public short ErrorCode { get; }
        public short UrlNumber { get; }
        public string Referer { get; }
    }
}