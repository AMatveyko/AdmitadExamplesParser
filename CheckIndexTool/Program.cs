using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AdmitadSqlData.Helpers;
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

            KillAllEdges();
            
            int.TryParse( args.Length > 0 ? args[0] : "0", out var urlNumber );
            int.TryParse( args.Length > 0 ? args[1] : "0", out var amountWorkers);
            
            Start( urlNumber, amountWorkers );
        }

        private static void Start(int urlNumber, int amountWorkers) {


            var infos = Measure(() => GetInfos(urlNumber, amountWorkers), out var seconds);
            var infosWithoutErrors = infos.Where(i => i.Error == null).ToList();

            SaveResult(infosWithoutErrors);
            
            WriteStatistics(seconds, infos, amountWorkers);
            
        }

        private static List<UrlIndexInfo> GetInfos(int urlNumber, int amountWorkers) {
            var dbSettings = SettingsBuilder.GetDbSettings();
            var repository = new TheStoreRepository(dbSettings);
            var settings = new SettingsBuilder(repository).GetSettings();
            var client = new UrlStatisticsIndexClient(settings.ElasticSearchClientSettings, new BackgroundBaseContext("",""));
            var worker = new CheckIndexWorker(client);
            return worker.CheckUrls(urlNumber > 0 ? urlNumber : 1000, amountWorkers > 0 ? amountWorkers : 5);
        }

        private static void WriteStatistics(int seconds, List<UrlIndexInfo> infos, int amountWorkers) {
            var infosWithErrors = infos.Where(i => i.Error != null).Select( i => $"{DateTime.Now} {i.Url}: {i.Error}").ToList();
            
            File.AppendAllLines(@"C:\tasks\checkIndexTool\errors.txt", infosWithErrors );

            var stats = 
                $"{DateTime.Now}, {seconds} sec, total {infos.Count}, errors {infosWithErrors.Count}, workers {amountWorkers}, % errors { infosWithErrors.Count / (infos.Count / 100)}";
            File.AppendAllLines(@"C:\tasks\checkIndexTool\statistics.txt", new []{ stats } );
        }
        
        private static void SaveResult(List<UrlIndexInfo> results) {
            var request = new SaveCheckingResultRequest("https://thestore.ru", results);
            var result = request.Execute();
        }

        private static T Measure<T>(Func<T> func, out int seconds) {
            var sw = new Stopwatch();
            sw.Start();
            var result = func();
            sw.Stop();
            seconds = (int)(sw.ElapsedMilliseconds / 1000);
            return result;
        }

        private static void KillAllEdges() {
            foreach (var process in Process.GetProcessesByName("msedge.exe")) {
                process.Kill();
            }
        }
    }
}