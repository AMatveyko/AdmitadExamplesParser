using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CheckIndexTool.Responses;
using CheckIndexTool.workers;
using Common.Api;
using Common.Elastic.Workers;
using Common.Entities;
using Common.Workers;
using TheStore.Api.Front.Data.Repositories;

namespace CheckIndexTool
{
    class Program
    {
        static void Main(string[] args) {
            // HideThisProcess();
            
            
            int.TryParse( args.Length > 0 ? args[0] : "0", out var urlNumber );
            int.TryParse( args.Length > 0 ? args[1] : "0", out var amountWorkers);
            
            Start( urlNumber, amountWorkers );
        }

        private static void Start(int urlNumber, int amountWorkers) {
            var dbSettings = SettingsBuilder.GetDbSettings();
            var repository = new TheStoreRepository(dbSettings);
            var settings = new SettingsBuilder(repository).GetSettings();
            var client = new UrlStatisticsIndexClient(settings.ElasticSearchClientSettings, new BackgroundBaseContext("",""));
            var worker = new CheckIndexWorker(client);
            var infos = worker.CheckUrls(urlNumber > 0 ? urlNumber : 1000, amountWorkers > 0 ? amountWorkers : 5);

            var infosWithoutErrors = infos.Where(i => i.Error == null).ToList();
            var infosWithErrors = infos.Where(i => i.Error != null).Select( i => $"{i.Url}: {i.Error}");
            
            File.AppendAllLines(@"o:\admitad\logs\indexChecker\errors.txt", infosWithErrors );
            
            SaveResult(infosWithoutErrors);
            
        }

        private static void SaveResult(List<UrlIndexInfo> results) {
            var request = new SaveCheckingResultRequest("https://thestore.ru", results);
            var result = request.Execute();
        }

        private static void HideThisProcess() {
            var currentProcess = Process.GetCurrentProcess();
            currentProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        }
    }
}