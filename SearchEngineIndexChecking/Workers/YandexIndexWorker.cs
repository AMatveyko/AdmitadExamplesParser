﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Entities;
using OpenQA.Selenium;
using SearchEngineIndexChecking.Parsers;
using SearchEngineIndexChecking.SeleniumScenarios;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class YandexIndexWorker
    {

        private Queue<IWebDriver> _browsers;
        private readonly object _lockFlag = new object();
        private readonly int _browserNumber;
        private readonly List<Task<UrlIndexInfo>> _tasks = new List<Task<UrlIndexInfo>>();

        private readonly WebDriverBuilder _builder;

        public YandexIndexWorker(WebDriverBuilder builder, int browserNumber) {
            (_builder, _browserNumber) = (builder, browserNumber);
            Initialize();
        }


        private void Initialize() {

            _browsers = new Queue<IWebDriver>();

            var tasks = new List<Task<IWebDriver>>();

            for (var i = 0; i < _browserNumber; i++) {
                tasks.Add(Task.Factory.StartNew(() => _builder.CreateBrowser()));
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks) {
                _browsers.Enqueue(task.Result);
            }
        }

        private void TryCheckUrlInTask(string url, IWebDriver browser) =>
            _tasks.Add(Task.Factory.StartNew(() => TryCheckUrl(url, browser)));

        private UrlIndexInfo TryCheckUrl(string url, IWebDriver browser) {
            try {
                return CheckUrl(url, browser);
            }
            catch (Exception e) {
                return new UrlIndexInfo {
                    Url = url,
                    Error = e.Message
                };
            }
        }

        public List<UrlIndexInfo> CheckUrls(List<string> urls) {

            foreach (var url in urls) {
                DoCheckUrl(url);
            }

            Task.WaitAll(_tasks.ToArray());
            
            Finish();

            return _tasks.Select(t => t.Result).ToList();
        }


        private void DoCheckUrl(string url) {
            var browser = GetBrowserFromPool();
            while (browser == null ) {
                Thread.Sleep(100);
                browser = GetBrowserFromPool();
            }

            TryCheckUrlInTask(url, browser);
        }

        private IWebDriver GetBrowserFromPool() {
            lock (_lockFlag) {
                return _browsers.Count > 0 ? _browsers.Dequeue() : null;
            }
        }


        private void PutBrowserPool(IWebDriver browser) {
            lock (_lockFlag) {
                _browsers.Enqueue(browser);
            }
        }
            

        private UrlIndexInfo CheckUrl(string url, IWebDriver browser) {
            var scenario = new YandexScenario(browser);
            var data = scenario.GetIndexInfo(url);
            PutBrowserPool( browser );
            var result = YandexParser.IsIndexed(data);
            var info = new UrlIndexInfo {
                Url = url,
                IsIndexed = result
            };

            return info;
        }

        private void Finish() {
            CloseAllBrowsers();
            KillAllFireFox();
        }

        private void CloseAllBrowsers() {
            var tasks = _browsers.Select(b => Task.Factory.StartNew(b.Quit)).ToArray();
            Task.WaitAll(tasks);
        }
        
        private static void KillAllFireFox() {
            var localByName = Process.GetProcessesByName("firefox");
            foreach(var p in localByName)
            {
                p.Kill();
            }
        }
    }
}