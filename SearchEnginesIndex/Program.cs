using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SearchEngineIndexChecking.Workers;


namespace SearchEnginesIndex
{
    class Program
    {
        
        
        private const string StartPage = "https://yandex.ru/";
        private const string QueryStringSelector = "[name = 'text']";
        private const string SearchButtonPath =
            "//div[@class='search2__button']//button[@class='button mini-suggest__button button_theme_search button_size_search i-bem button_js_inited']";

        private const string CaptchaButtonPath = "//input[@class='CheckboxCaptcha-Button']";
        private const string ResultFieldPath = "//div[@class='serp-adv__found']";
        
        static void Main(
            string[] args) {

            var driver = CreateEdge();
            
            

        }
        
        private static IWebDriver CreateEdge() {
            var service = EdgeDriverService.CreateDefaultService(@"o:\admitad\edgeWebDriver\","msedgedriver.exe");
            var options = new EdgeOptions();
            // options.AddArgument("--headless");
            var driver = new EdgeDriver(service, options);
            return driver;
        }
        
    }
}