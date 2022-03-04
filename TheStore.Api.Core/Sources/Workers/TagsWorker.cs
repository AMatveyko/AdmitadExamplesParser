// a.snegovoy@gmail.com

using System;
using System.Linq;
using Admitad.Converters.Workers;
using AdmitadCommon.Entities;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Settings;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class TagsWorker : BaseLinkWorker
    {

        public TagsWorker( ElasticSearchClientSettings settings, BackgroundWorks works, DbHelper dbHelper, ProductRatingCalculation productRatingCalculation ) 
            :base( settings, works, dbHelper, productRatingCalculation ) { }

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