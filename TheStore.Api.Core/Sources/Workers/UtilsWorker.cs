// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities.Api;


using TheStore.Api.Core.Sources.Helpers;
using TheStore.Api.Front.Data.Repositories;

namespace TheStore.Api.Core.Sources.Workers
{
    public sealed class UtilsWorker
    {

        private readonly TheStoreRepository _repository;

        public UtilsWorker(
            TheStoreRepository repository ) =>
            _repository = repository;
        
        public void ComparePages( CompareProjectsContext context )
        {
            var infos = new UrlHelper().GetInfos();
            context.TotalActions = infos.Count + 1; //1 для записи в бд. чтобы не получилось 100% до того как запишем.
            context.AddMessage( $"Получили { infos.Count } страниц" );
            var worker = new CompareWorker( _repository, context );
            worker.CompareAndWrite( infos );
        }
    }
}