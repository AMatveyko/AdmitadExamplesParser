
using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class WebDriverBuilder
    {

        private const int ImplicitlyWait = 2;

        public IWebDriver CreateBrowser() {
            var driver = CreateEdge();
            driver.Manage().Window.Minimize();
            
            return driver;
        }

        private static IWebDriver CreateEdge() {
            var service = EdgeDriverService.CreateDefaultService(@"C:\webdriver","msedgedriver.exe");
            var options = new EdgeOptions();
            options.AddArgument("--disk-cache-dir=null");
            var driver = new EdgeDriver(service, options);
            return driver;
        }
        
        private static IWebDriver CreateGoogle() {
            var service =
                ChromeDriverService.CreateDefaultService(@"c:\Program Files\Google\Chrome\Application\","chromedriver.exe");
            var driver = new ChromeDriver( service );
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplicitlyWait);
            return driver;
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
                var driver = new FirefoxDriver(service, options );
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplicitlyWait);
                return driver;
        }
    }
}