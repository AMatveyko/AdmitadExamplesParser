using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SearchEnginesIndex
{
    class Program
    {
        static void Main(
            string[] args )
        {

            var urls = new List<string> {
                "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_platforme/",
                "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_shejnye/",
                "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_dragoczennye/",
                "https://thestore.ru/dlya/zhenschin/belyo/kupit_setka/",
                "https://thestore.ru/dlya/zhenschin/odezhda/kupit_so_skidkoy/",
                "https://thestore.ru/dlya/zhenschin/aksessuary/dlya_volos/",
                "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_tanketke/",
                "https://thestore.ru/dlya/zhenschin/odezhda/kostumy/",
                "https://thestore.ru/dlya/zhenschin/aksessuary/platki/",
                "https://thestore.ru/dlya/zhenschin/obuv/kedy/",
                "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_platforme/",
                "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_shejnye/",
                "https://thestore.ru/dlya/zhenschin/ukrasheniya/kupit_dragoczennye/",
                "https://thestore.ru/dlya/zhenschin/belyo/kupit_setka/",
                "https://thestore.ru/dlya/zhenschin/odezhda/kupit_so_skidkoy/",
                "https://thestore.ru/dlya/zhenschin/aksessuary/dlya_volos/",
                "https://thestore.ru/dlya/zhenschin/obuv/kupit_na_tanketke/",
                "https://thestore.ru/dlya/zhenschin/odezhda/kostumy/",
                "https://thestore.ru/dlya/zhenschin/aksessuary/platki/",
                "https://thestore.ru/dlya/zhenschin/obuv/kedy/"
            };

            var sw = new Stopwatch();
            sw.Start();

            var tasks = urls.Select( u => Task.Factory.StartNew( () => GetUrls( u ) ) ).ToArray();

            Task.WaitAll( tasks );

            sw.Stop();
            var time = sw.ElapsedMilliseconds;
            
            var results = tasks.Select( t => t.Result ).ToList();

        }

        private static string GetUrls( string url )
        {

            var driver = CreateDriver();
            var wait = new WebDriverWait( driver, TimeSpan.FromSeconds(5) );
            
            driver.Url = "https://yandex.ru/";
            
            var searchText = wait.Until( d => d.FindElement(By.CssSelector("[name = 'text']")) );
 
            searchText.SendKeys($"site:{url}");

            var searchButton = wait.Until( d => d.FindElement(By.XPath("//div[@class='search2__button']//button[@class='button mini-suggest__button button_theme_search button_size_search i-bem button_js_inited']")));
 
            searchButton.Click();

            if( IsCaptcha( driver ) ) {
                ClickCaptcha( driver);
                ;
            }

            if( IsMisspell( driver ) ) {
                return "Ничего не найдено";
            }
            
            //var result = wait.Until( d =>  );
            //return result.Text;

            return GetResult( driver );
        }

        private static string GetResult(
            IWebDriver driver ) =>
            driver.FindElement( By.XPath( "//div[@class='serp-adv__found']" ) ).Text; 
        
        private static IWebDriver CreateDriver() {
            var options = new ChromeOptions();
            options.AddArgument( "--headless" );
            return new ChromeDriver( options );
        }
        
        private static bool IsCaptcha(
            IWebDriver driver ) =>
            driver.PageSource.Contains("CheckboxCaptcha-Button");
//                                      CheckboxCaptcha-Button

        private static void ClickCaptcha( ISearchContext driver ) {

            var captchaButton = driver.FindElement(
                By.XPath(
                    "//input[@class='CheckboxCaptcha-Button']" ) );
//                                   CheckboxCaptcha-Button

            captchaButton.Click();
        }

        private static bool IsMisspell(
            IWebDriver driver ) =>
            driver.PageSource.Contains( "По вашему запросу ничего не нашлось" );
    }
}