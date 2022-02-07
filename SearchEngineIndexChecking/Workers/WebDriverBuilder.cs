
using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class WebDriverBuilder
    {

        private const int ImplicitlyWait = 2;

        public IWebDriver CreateBrowser() {
            var driver = CreateFirefox();
            //var driver =  CreateGoogle();
            //Thread.Sleep(2000);
            // driver.Url = "http://localhost:8080/swagger/index.html";
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
                // var profile = new FirefoxProfile();
                // profile.SetPreference("permissions.default.image","2");
                var options = new FirefoxOptions();
                // options.Profile = profile;
                //options.AddArgument("--headless");
                options.SetPreference("permissions.default.image","2");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-setuid-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                // options.AddArgument("--window-size-minimize_window");
                var driver = new FirefoxDriver(service, options );
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplicitlyWait);
                return driver;
        }
    }
}