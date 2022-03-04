using System.Collections.Generic;
using Common.Entities;
using SearchEngineIndexChecking.Entities;

namespace SearchEngineIndexChecking.Workers
{
    public static class IndexChecker
    {

        public static List<UrlIndexInfo> CheckUrls(List<string> urls, int amountWorkers) {
            var worker = GetWorker(amountWorkers);
            
            return worker.CheckUrls(urls);
        }

        private static YandexIndexWorker GetWorker(int amountWorkers) {
            var builder = new WebDriverBuilder();
            var distributor = GetDistributor( builder, amountWorkers );
            var wordSet = new DictionaryWorker();
            
            return new YandexIndexWorker( distributor, wordSet );
        }

        private static IBrowserDistributor GetDistributor(IBrowserBuilder builder, int amountWorkers) {
            
            var types = new List<BrowserType> {
                BrowserType.Edge,
                // BrowserType.Google
            };

            var setBuilder = GetSetBuilder(types, false);
            
            return new BrowserDistributor(builder, setBuilder, amountWorkers );
        }

        private static IBrowserTypesSetBuilder GetSetBuilder(List<BrowserType> types, bool isRandom) =>
            isRandom
                ? (IBrowserTypesSetBuilder)new RandomTypesSetBuilder(types)
                : new OrderedTypesSetBuilder( types );
    }
}