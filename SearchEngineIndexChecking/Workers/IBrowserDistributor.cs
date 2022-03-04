using System.Collections.Generic;
using OpenQA.Selenium;
using SearchEngineIndexChecking.Entities;

namespace SearchEngineIndexChecking.Workers
{
    internal interface IBrowserDistributor
    {
        Queue<IWebDriver> GetBrowsers();
    }
}