using SearchEngineIndexChecking.Entities;

namespace SearchEngineIndexChecking.Workers
{
    internal interface IBrowserTypesSetBuilder
    {
        BrowserType GetNextType();
    }
}