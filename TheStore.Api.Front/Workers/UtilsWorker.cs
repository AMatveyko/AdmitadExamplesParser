// a.snegovoy@gmail.com

using System;
using System.Diagnostics;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using TheStore.Api.Front.Data.Repositories;
using TheStore.Api.Front.Helpers;

namespace TheStore.Api.Front.Workers
{
    internal sealed class UtilsWorker
    {

        private readonly TheStoreRepository _repository;

        public UtilsWorker(
            TheStoreRepository repository ) =>
            _repository = repository;
        
        public IActionResult ComparePages( int? visits )
        {
            try {
                var infos = new UrlHelper().GetInfos( visits );
                var worker = new CompareWorker( _repository );
                var sw = new Stopwatch();
                sw.Start();
                worker.CompareAndWrite( infos );
                sw.Stop();
                var ts = TimeSpan.FromMilliseconds( sw.ElapsedMilliseconds );
                return new ContentResult {
                    Content = $"Ok. { ts.TotalSeconds }s"
                };
            }
            catch( Exception e ) {
                return new ContentResult {
                    Content = e.Message
                };
            }
        }
    }
}