// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Entities;

namespace TheStore.Api.Core.Sources.Workers
{
    internal interface IUrlStatisticsWorker
    {
        public void Update( string url, BotType botType, short? errorCode );
        public void AddUrls( List<string> urls );
    }
}