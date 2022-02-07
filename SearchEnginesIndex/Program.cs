using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenQA.Selenium;
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

            // var urls = new List<string> {
            //     "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_platforme/",
            //     "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_shejnye/",
            //     "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_shejnye/sdfasdfasdfadfa/asdfadsf",
            //     "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_dragoczennye/",
            //     "https://thestore.ru/dlya/zhenschin/belyo/kupit_setka/",
            //     "https://thestore.ru/dlya/zhenschin/odezhda/kupit_so_skidkoy/",
            //     "https://thestore.ru/dlya/zhenschin/aksessuary/dlya_volos/",
            //     "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_tanketke/",
            //     "https://thestore.ru/dlya/zhenschin/odezhda/kostumy/",
            //     "https://thestore.ru/dlya/zhenschin/aksessuary/platki/",
            //     "https://thestore.ru/dlya/zhenschin/obuv/kedy/",
            //     "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_platforme/",
            //     "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_shejnye/",
            //     "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_dragoczennye/",
            //     "https://thestore.ru/dlya/zhenschin/belyo/kupit_setka/",
            //     "https://thestore.ru/dlya/zhenschin/odezhda/kupit_so_skidkoy/",
            //     "https://thestore.ru/dlya/zhenschin/aksessuary/dlya_volos/",
            //     "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_tanketke/",
            //     "https://thestore.ru/dlya/zhenschin/odezhda/kostumy/",
            //     "https://thestore.ru/dlya/zhenschin/aksessuary/platki/",
            //     "https://thestore.ru/dlya/zhenschin/obuv/kedy/"
            // };
            //
            var urls2 = new List<string>() {
                "https://thestore.ru/dlya/zhenschin/color-aivori/",
                "https://thestore.ru/dlya/zhenschin/brand-wisell/",
                "https://thestore.ru/dlya/zhenschin/color-liloviy/",
                "https://thestore.ru/dlya/zhenschin/shop-respectshoes/",
                "https://thestore.ru/dlya/zhenschin/brand-seed/",
                "https://thestore.ru/dlya/zhenschin/brand-intimissimi/",
                "https://thestore.ru/dlya/zhenschin/shop-reebok/",
                "https://thestore.ru/dlya/zhenschin/brand-pilyq/",
                "https://thestore.ru/dlya/zhenschin/brand-keepsake/",
                "https://thestore.ru/dlya/zhenschin/brand-catisa/",
                "https://thestore.ru/dlya/zhenschin/brand-hermitage/",
                "https://thestore.ru/dlya/zhenschin/brand-altea/",
                "https://thestore.ru/dlya/zhenschin/color-slivoviy/",
                "https://thestore.ru/dlya/zhenschin/shop-birkenstock/",
                "https://thestore.ru/dlya/zhenschin/shop-marksandspencer/",
                "https://thestore.ru/dlya/zhenschin/color-lazurniy/",
                "https://thestore.ru/dlya/zhenschin/sostav-poliuretan/",
                "https://thestore.ru/dlya/zhenschin/sostav-hrustal/",
                "https://thestore.ru/dlya/zhenschin/obuv/kupit_serebristye/",
                "https://thestore.ru/dlya/zhenschin/shop-newbalance/",
                "https://thestore.ru/dlya/zhenschin/brand-memjs/",
                "https://thestore.ru/dlya/zhenschin/shop-beru/",
                "https://thestore.ru/dlya/zhenschin/brand-milly/",
                "https://thestore.ru/dlya/zhenschin/color-terrakotoviy/",
                "https://thestore.ru/dlya/zhenschin/aksessuary/koshelki/kupit_na_vysokoi_platforme/",
                "https://thestore.ru/dlya/zhenschin/brand-roobins/",
                "https://thestore.ru/dlya/zhenschin/size-56/",
                "https://thestore.ru/dlya/zhenschin/odezhda/brand-sarman/",
                "https://thestore.ru/dlya/zhenschin/belyo/brand-valentina/",
                "https://thestore.ru/dlya/zhenschin/odezhda/brand-tigerlily/",
                "https://thestore.ru/dlya/zhenschin/brand-manas/",
                "https://thestore.ru/dlya/zhenschin/size-140/",
                "https://thestore.ru/dlya/zhenschin/odezhda/brand-noemi/",
                "https://thestore.ru/dlya/zhenschin/belyo/sorochki_neglizhe/kupit_dlya_leta/",
                "https://thestore.ru/dlya/zhenschin/ukrasheniya/kolca/kupit_obruchalnye/",
                "https://thestore.ru/dlya/zhenschin/brand-tagliatore/",
                "https://thestore.ru/dlya/zhenschin/sostav-tekstil/",
                "https://thestore.ru/dlya/zhenschin/sostav-denim/",
                "https://thestore.ru/dlya/zhenschin/brand-seventy/",
                "https://thestore.ru/dlya/zhenschin/shop-tamaris/",
                "https://thestore.ru/dlya/zhenschin/brand-altea/",
                "https://thestore.ru/dlya/zhenschin/size-140/",
                "https://thestore.ru/dlya/zhenschin/brand-majorelle/",
                "https://thestore.ru/dlya/zhenschin/color-aivori/",
                "https://thestore.ru/dlya/zhenschin/krasota/kupit_dorogo/",
                "https://thestore.ru/dlya/zhenschin/odezhda/",
                "https://thestore.ru/dlya/zhenschin/shop-svmoscow/",
                "https://thestore.ru/dlya/zhenschin/brand-moncler/",
                "https://thestore.ru/dlya/zhenschin/sostav-tvid/",
                "https://thestore.ru/dlya/zhenschin/brand-milly/"
            };
            //
            // var sw = new Stopwatch();
            // sw.Start();
            //
            // var results = IndexChecker.CheckUrls(urls2, 5);
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            // // results.AddRange( IndexCheckerTool.CheckUrls(urls2));
            //
            //
            // sw.Stop();
            // var seconds = sw.ElapsedMilliseconds / 1000;

            // Start();
            
            
            
            
            
            
            //var dbSettings = SettingsBuilder.GetDbSettings();
            //var repository = new TheStoreRepository(dbSettings);
            //var settings = new SettingsBuilder(repository).GetSettings();
            //var client = new UrlStatisticsIndexClient(settings.ElasticSearchClientSettings, new BackgroundBaseContext("",""));
            // var urls = client.GetUrlsForChecking(BotType.Yandex, 1000);
            // var infos = IndexChecker.CheckUrls(urls2, 5);
            //
            // var infosWithoutErrors = infos.Where(i => i.Error == null).ToList();
            // var infosWithErrors = infos.Where(i => i.Error != null).Select( i => $"{i.Url}: {i.Error}");
            //
            // File.AppendAllLines(@"o:\admitad\logs\indexChecker\errors.txt", infosWithErrors );
            //
            // var request = new SaveCheckingResultRequest("https://thestore.ru", infosWithoutErrors);
            // var result = request.Execute();


            var task = Task.Factory.StartNew(Test);
            task.Wait();
            

        }

        // private static void Start() {
        //     var dbSettings = SettingsBuilder.GetDbSettings();
        //     var repository = new TheStoreRepository(dbSettings);
        //     var settings = new SettingsBuilder(repository).GetSettings();
        //     var client = new UrlStatisticsIndexClient(settings.ElasticSearchClientSettings, new BackgroundBaseContext("",""));
        //     var worker = new CheckIndexWorker(client);
        //     var infos = worker.CheckUrls(1000, 5);
        //
        //     var infosWithoutErrors = infos.Where(i => i.Error == null).ToList();
        //     var infosWithErrors = infos.Where(i => i.Error != null).Select( i => $"{i.Url}: {i.Error}");
        //     
        //     File.AppendAllLines(@"o:\admitad\logs\indexChecker\errors.txt", infosWithErrors );
        //     
        //     var request = new SaveCheckingResultRequest("https://thestore.ru", infosWithoutErrors);
        //     var result = request.Execute();
        // }


        private static void Test() {
            var service = 
                FirefoxDriverService.CreateDefaultService(@"c:\Program Files\Mozilla Firefox\", "geckodriver.exe");
            service.FirefoxBinaryPath = @"c:\Program Files\Mozilla Firefox\firefox.exe";
            var options = new FirefoxOptions();
            // options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-setuid-sandbox");
            // options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--window-size-minimize_window");
            var driver = new FirefoxDriver(service, options );
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds( 2 );
            driver.Navigate().GoToUrl("http://yandex.ru");
            DoGetInfo( "https://thestore.ru/dlya/zhenschin/brand-seventy/", driver);
        }
        
        private static string DoGetInfo(string url, IWebDriver _browser ) {
            
            // _browser.Url = StartPage;
            // _browser.Navigate().GoToUrl( StartPage );
            // _browser.Navigate().GoToUrl("http://localhostl:8080");

            SetQuery(url, _browser);
            Search(_browser);
            
            if (IsCaptcha(_browser)) {
                
                Console.WriteLine("Captcha!");
                
                ClickCaptcha(_browser);
            }

            return _browser.PageSource;
        }
        
        private static void SetQuery(string url, IWebDriver _browser ) {
            var searchText = _browser.FindElement(By.CssSelector( QueryStringSelector ));
            // searchText.SendKeys($"site:{url}");
            // searchText.SendKeys(url);
            searchText.SendKeys($"url:{url}");
        }
        
        private static void Search( IWebDriver _browser) {
            var searchButton = _browser.FindElement(By.XPath(SearchButtonPath));
            searchButton.Click();
        }
        
        private static bool IsCaptcha(IWebDriver _browser) => _browser.PageSource.Contains("CheckboxCaptcha-Button");

        private static void ClickCaptcha( IWebDriver _browser) {
            var captchaButton = _browser.FindElement(By.XPath(CaptchaButtonPath) );
            captchaButton.Click();
        } 
        
    }
}