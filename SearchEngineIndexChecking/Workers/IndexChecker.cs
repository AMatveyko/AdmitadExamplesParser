using System;
using System.Collections.Generic;
using Common.Entities;

namespace SearchEngineIndexChecking.Workers
{
    public static class IndexChecker
    {

        public static List<UrlIndexInfo> CheckUrls(List<string> urls, int amountWorkers) {
            var builder = new WebDriverBuilder();
            var worker = new YandexIndexWorker( builder, amountWorkers );
            return worker.CheckUrls(urls);
        }
    }
}