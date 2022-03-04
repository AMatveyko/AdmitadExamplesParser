using System.Collections.Generic;
using NUnit.Framework;
using SearchEngineIndexChecking.Entities;
using SearchEngineIndexChecking.SeleniumScenarios;
using SearchEngineIndexChecking.Workers;

namespace SearchEngineIndexCheckingTests
{
    public class ScenarioTests
    {
        [Test]
        public void YandexScenarioTest() {
            const string url = "https://thestore.ru/dlya/zhenschin/odezhda/verhnya/";
            var builder = new WebDriverBuilder();
            var types = new List<BrowserType> {BrowserType.Edge};
            var setBuilder = new OrderedTypesSetBuilder(types);
            var distributor = new BrowserDistributor(builder, setBuilder, 1 );
            var drivers = distributor.GetBrowsers();
            var scenario = new YandexScenario(drivers.Dequeue(), new DictionaryWorker());
            var data = scenario.GetIndexInfo(url);
            
        }
    }
}