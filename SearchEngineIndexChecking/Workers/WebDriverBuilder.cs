
using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using SearchEngineIndexChecking.Entities;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class WebDriverBuilder : IBrowserBuilder
    {
        
        private const int ImplicitlyWait = 5;

        public IWebDriver CreateBrowser( BrowserType type ) {

            var driver = CreateTypedBrowser(type);
            var configuredDriver = GetConfiguredDriver(driver);
            return configuredDriver;
        }

        private static IWebDriver GetConfiguredDriver(IWebDriver driver) {
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplicitlyWait);
            return driver;
        }

        private static IWebDriver CreateTypedBrowser( BrowserType type ) =>
            type switch {
                BrowserType.Google => CreateGoogle(),
                BrowserType.Edge => CreateEdge(),
                BrowserType.FireFox => CreateFirefox(),
                _ => throw new AggregateException("unknown browser type" )
            };

        private static IWebDriver CreateEdge() {
            var service = EdgeDriverService.CreateDefaultService(@"C:\webdriver","msedgedriver.exe");
            var options = new EdgeOptions();
            return new EdgeDriver(service, options);
        }
        
        private static IWebDriver CreateGoogle() {
            var service =
                ChromeDriverService.CreateDefaultService(@"c:\Program Files\Google\Chrome\Application\","chromedriver.exe");

            var options = new ChromeOptions();
            return new ChromeDriver( service, options );;
        }
        
        private static IWebDriver CreateFirefox() {
            var service = 
                FirefoxDriverService.CreateDefaultService(@"c:\Program Files\Mozilla Firefox\", "geckodriver.exe");
                service.FirefoxBinaryPath = @"c:\Program Files\Mozilla Firefox\firefox.exe";
                var options = new FirefoxOptions();
                options.SetPreference("permissions.default.image","2");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-setuid-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                return new FirefoxDriver(service, options );
        }
    }
}