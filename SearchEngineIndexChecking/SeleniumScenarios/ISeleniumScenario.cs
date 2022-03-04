namespace SearchEngineIndexChecking.SeleniumScenarios
{
    internal interface ISeleniumScenario
    {
        string GetIndexInfo(string url);
    }
}