using System.Collections.Generic;
using System.Threading.Tasks;
using OpenQA.Selenium;
using SearchEngineIndexChecking.Entities;

namespace SearchEngineIndexChecking.Workers
{
    internal sealed class BrowserDistributor : IBrowserDistributor
    {
        private readonly IBrowserBuilder _builder;
        private readonly int _number;
        private readonly IBrowserTypesSetBuilder _setBuilder;

        public BrowserDistributor(IBrowserBuilder builder, IBrowserTypesSetBuilder setBuilder, int number ) =>
            (_builder, _number, _setBuilder) =
            (builder, number, setBuilder );

        public Queue<IWebDriver> GetBrowsers() {
            var tasks = GetBrowsersWaiters();
            Task.WaitAll(tasks);

            return GetBrowsersSet(tasks);
        }

        private static Queue<IWebDriver> GetBrowsersSet(Task<IWebDriver>[] tasks) {
            var queue = new Queue<IWebDriver>();
            foreach (var task in tasks) {
                queue.Enqueue(task.Result);
            }

            return queue;
        }

        private Task<IWebDriver>[] GetBrowsersWaiters() {       
            var tasks = new List<Task<IWebDriver>>();

            for (var i = 0; i < _number; i++) {
                var type = _setBuilder.GetNextType();
                tasks.Add( CreateBrowserWaiter(type) );
            }
        
            return tasks.ToArray();
        }
        
        private Task<IWebDriver> CreateBrowserWaiter(BrowserType type) =>
            Task.Factory.StartNew(() => _builder.CreateBrowser(type));

    }
}