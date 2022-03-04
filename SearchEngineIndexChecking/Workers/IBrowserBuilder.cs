using OpenQA.Selenium;
using SearchEngineIndexChecking.Entities;

namespace SearchEngineIndexChecking.Workers
{
    internal interface IBrowserBuilder
    {
        IWebDriver CreateBrowser(BrowserType type);
    }
}