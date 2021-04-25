// a.snegovoy@gmail.com

using System;
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
            context.Title = tag.Title;
            var linker = CreateLinker( context );
            linker.RelinkTag( tag );
        }

        public void LinkTags( LinkTagsContext context )
        {
            throw new NotImplementedException();
        }

    }
}