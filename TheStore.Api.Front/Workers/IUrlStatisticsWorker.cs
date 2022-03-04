// a.snegovoy@gmail.com

using System.Collections.Generic;
using Common.Entities;
using TheStore.Api.Front.Entity;

namespace TheStore.Api.Front.Workers
{
    internal interface IUrlStatisticsWorker
    {
        public List<string> Update( UrlStatisticsParameters parameters );
        public void AddUrls( List<string> urls );
        public void SaveCheckingResult(List<UrlIndexInfo> infos);
    }
}