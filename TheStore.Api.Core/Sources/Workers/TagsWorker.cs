// a.snegovoy@gmail.com

using System;
using System.Linq;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;

using AdmitadSqlData.Helpers;

using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class TagsWorker : BaseLinkWorker
    {

        public TagsWorker( ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper dbHelper ) 
            :base( settings, works, dbHelper ) { }

        public void RelinkTag( RelinkTagContext context )
        {
            var tag = Db.GetTags().FirstOrDefault( t => t.Id == context.TagId );
            context.Title = tag.Title;
            var linker = CreateLinker( context );
            if( context.Relink ) {
                linker.RelinkTag( tag );
            }
            else {
                linker.LinkTag( tag );
            }
        }

        public void LinkTags( LinkTagsContext context )
        {
            var tags = Db.GetTags();
            var linker = CreateLinker( context );
            linker.LinkTags( tags );
        }

    }
}