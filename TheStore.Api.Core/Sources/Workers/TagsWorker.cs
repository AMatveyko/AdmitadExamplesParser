// a.snegovoy@gmail.com

using System;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class TagsWorker : BaseLinkWorker
    {

        public TagsWorker( ElasticSearchClientSettings settings, BackgroundWorks works ) 
            :base( settings, works ) { }

        public void RelinkTag( RelinkTagContext context )
        {
            var tag = DbHelper.GetTags().FirstOrDefault( t => t.Id == context.TagId );
            context.Title = tag.Title;
            var linker = CreateLinker( context );
            linker.RelinkTag( tag );
        }

        public void LinkTags( LinkTagsContext context )
        {
            var tags = DbHelper.GetTags();
            var linker = CreateLinker( context );
            linker.LinkTags( tags );
        }

    }
}