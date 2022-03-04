using System.Collections.Generic;
using Common.Elastic.Workers;
using Common.Entities;
using SearchEngineIndexChecking.Workers;

namespace CheckIndexTool.workers
{
    internal class CheckIndexWorker
    {
        private readonly UrlStatisticsIndexClient _indexClient;

        public CheckIndexWorker(UrlStatisticsIndexClient indexClient) => _indexClient = indexClient;

        public List<UrlIndexInfo> CheckUrls(int urlNumber, int amountWorkers) {

            var urls = _indexClient.GetUrlsForChecking(BotType.Yandex, urlNumber);
            var infos = IndexChecker.CheckUrls(urls, amountWorkers);
            return infos;
        }
    }
}