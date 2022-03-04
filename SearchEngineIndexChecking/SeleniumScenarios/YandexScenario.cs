using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using SearchEngineIndexChecking.Helpers;
using SearchEngineIndexChecking.Workers;

namespace SearchEngineIndexChecking.SeleniumScenarios
{
    internal sealed class YandexScenario : ISeleniumScenario
    {
        
        private static List<string> _startPages = new List<string> { "https://yandex.ru", "https://ya.ru" };
        private const string QueryStringSelector = "[name = 'text']";
        private const string SearchButtonPath =
            "//div[@class='search2__button']//button[@class='button mini-suggest__button button_theme_search button_size_search-large i-bem button_js_inited']";

        private const string SearchButtonPathSecond =
            "//div[@class='search2__button']//button[@class='websearch-button mini-suggest__button']";

        private const string SearchUrl = "https://yandex.ru/search/";

        private const string CaptchaButtonPath = "//input[@class='CheckboxCaptcha-Button']";
        private const string ResultFieldPath = "//div[@class='serp-adv__found']";

        private static List<string> _navigateWay = new List<string> {"main", "noMain"};

        private readonly RandomElementGetter<string> _randomElementGetter = new RandomElementGetter<string>();
        private readonly IWordsSet _wordsSet;


        private readonly IWebDriver _browser;

        public YandexScenario(IWebDriver driver, IWordsSet wordSet) => (_browser, _wordsSet) = (driver, wordSet);

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

        private string GetStartPage() => _randomElementGetter.Get(_startPages);
        
        private string DoGetInfo(string url) {
            
            Wait();
            
            GoToStartPageIfNeed();
            
            SearchRandomPhrase();

            GetUrlSearchResult(url);
            
            // Search();
            
            CheckAndClickCaptcha();

            return _browser.PageSource;
        }

        private void SearchRandomPhrase() {
            var phrase = _wordsSet.GetPhraseRandomLength();
            SetQuery(phrase);
            
            CheckAndClickCaptcha();
        }
        
        private void GetUrlSearchResult(string url) {
            var queryString = $"url:{url}";
            SetQuery(queryString);
        }
        
        private static void Wait() {
            var rand = new Random();
            var number = rand.Next(1,5);
            var mss = number * 50;
            Thread.Sleep( mss );
        }
        
        private void GoToStartPageIfNeed() {
            var startPage = GetStartPage();
            
            if( IsFirstStart() || SearchThroughMainPage() ) {
                _browser.Navigate().GoToUrl(startPage);
            }
        }

        private void CheckAndClickCaptcha() {
            
            if ( IsCaptcha() == false ) {
                return;
            }
            
            Console.WriteLine("Captcha!");
                
            ClickCaptcha();
        }
        
        private bool SearchThroughMainPage() =>
            _randomElementGetter.Get(_navigateWay) == "main";
        
        private bool IsFirstStart() => _browser.Url.Contains(SearchUrl) == false;

        private void SetQuery(string queryString) {
            var searchText = _browser.FindElement(By.CssSelector( QueryStringSelector ));
            searchText.Clear();
            searchText.SendKeys(queryString);
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