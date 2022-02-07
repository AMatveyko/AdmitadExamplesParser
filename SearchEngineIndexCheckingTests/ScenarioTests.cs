using NUnit.Framework;
using SearchEngineIndexChecking.SeleniumScenarios;
using SearchEngineIndexChecking.Workers;

namespace SearchEngineIndexCheckingTests
{
    public class ScenarioTests
    {
        [Test]
        public void YandexScenarioTest() {
            var url = "https://thestore.ru/dlya/zhenschin/odezhda/verhnya/";
            var builder = new WebDriverBuilder();
            var driver = builder.CreateBrowser();
            var scenario = new YandexScenario(driver);
            var data = scenario.GetIndexInfo(url);
            
        }
    }
}