// a.snegovoy@gmail.com

using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Workers;

using AdmitadSqlData.Helpers;

using TheStore.Api.Core.Sources.Entity;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class TagsWorker : BaseLinkWorker
    {

        public TagsWorker( ElasticSearchClientSettings settings ) 
            :base( settings ) { }

        public void RelinkTag( RelinkTagContext context )
        {
            var tag = DbHelper.GetTags().FirstOrDefault( t => t.Id == context.TagId );
            var client = CreateClient( context );
            var unlinkResult = client.UnlinkTag( tag );
            context.Messages.Add( $"Отвязали { unlinkResult.Pretty } товаров от тега" );
            context.PercentFinished = 50;
            var linkResult = client.UpdateProductsForTag( tag );
            context.Messages.Add( $"Привязвали { linkResult.Pretty } товаров к тегу" );
            context.PercentFinished = 100;
            
            context.Content = $"{tag.Id}: отвязали {unlinkResult.Pretty}, привязали {linkResult.Pretty}, разница { unlinkResult.GetDifferencePercent( linkResult ) }%";
        }

        public void LinkTags( LinkTagsContext context )
        {
            
        }

    }
}