using System;
using OpenQA.Selenium;

namespace SearchEngineIndexChecking.SeleniumScenarios
{
    internal sealed class YandexScenario : ISeleniumScenario
    {

        // private const string StartPage = "https://yandex.ru/";
        private const string StartPage = "https://ya.ru";
        private const string QueryStringSelector = "[name = 'text']";
        // private const string SearchButtonPath =
        //     "//div[@class='search2__button']//button[@class='button mini-suggest__button button_theme_search button_size_search i-bem button_js_inited']";
        private const string SearchButtonPath =
            "//div[@class='search2__button']//button[@class='button mini-suggest__button button_theme_search button_size_search-large i-bem button_js_inited']";

        private const string SearchButtonPathSecond =
            "//div[@class='search2__button']//button[@class='websearch-button mini-suggest__button']";

        private const string SearchUrl = "https://yandex.ru/search/";
        
        
        // 

        private const string CaptchaButtonPath = "//input[@class='CheckboxCaptcha-Button']";
        private const string ResultFieldPath = "//div[@class='serp-adv__found']";

        
        private readonly IWebDriver _browser;

        public YandexScenario(IWebDriver driver) => _browser = driver;

        public string GetIndexInfo(string url) =>
            TryGetInfo(url);

        private string TryGetInfo(string url) {
            try {
                return DoGetInfo(url);
            }
            catch (Exception e) {
                return $"error:{ e.Message }";
            }
        }

        private string DoGetInfo(string url) {
            if( IsFirstStart() ) {
                _browser.Navigate().GoToUrl(StartPage);
            }

            SetQuery(url);
            // Search();
            
            if (IsCaptcha()) {
                
                Console.WriteLine("Captcha!");
                
                ClickCaptcha();
            }

            return _browser.PageSource;
        }

        private bool IsFirstStart() => _browser.Url.Contains(SearchUrl) == false;

        private void SetQuery(string url) {
            var searchText = _browser.FindElement(By.CssSelector( QueryStringSelector ));
            searchText.Clear();
            searchText.SendKeys($"url:{url}");
            ClickEnter( searchText );
        }

        private void ClickEnter(IWebElement element) {
            element.SendKeys(Keys.Enter);
        }
        
        private void Search() {
            var searchButton = _browser.FindElement(By.XPath(SearchButtonPath));
            searchButton.Click();
        }
        
        private bool IsCaptcha() => _browser.PageSource.Contains("CheckboxCaptcha-Button");

        private void ClickCaptcha() {
            var captchaButton = _browser.FindElement(By.XPath(CaptchaButtonPath) );
            captchaButton.Click();
        }
    }
}