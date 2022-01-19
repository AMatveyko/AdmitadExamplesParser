// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Text;

using Admitad.Converters.Entities;

using Common.Api;
using Common.Entities;

namespace Admitad.Converters.Workers
{
    public sealed class FeedParser : BaseFeedParser
    {
        public FeedParser(
            IMinimalDownloadInfo downloadInfo,
            BackgroundBaseContext context )
            : base( downloadInfo, context ) { }

        protected override void PrepareOffers( bool isOnlyCategories ) {

            var formattedLines = Measure( () => GetFormattedOfferLinesWithProcessFile(isOnlyCategories), out var getLinesTime );
            
            Measure( () => ParseOffers( formattedLines ), out var serializeTime );
            AddStatistics( formattedLines.Count, getLinesTime, serializeTime );
        }

        private void ParseOffers( List<string> lines ) {
            lines.ForEach( l => ProcessString(GetCollector, l) );
        }

        private List<RawOffer> GetCollector(RawOffer offerRaw) =>
            offerRaw.IsDeleted
                ? ShopData.DeletedOffers
                : ShopData.NewOffers;
        
        private List<string> GetFormattedOfferLinesWithProcessFile(bool isOnlyCategoriesNeed) {
            var formattedLines = new LinesBufferInList();
            var buffer = new StringBuilder();
            var firstOfferLine = GetFirstOfferLine( FilePath );
            var lineNumber = 0;

            var func = new Func<string, bool>(
                line => ProcessLineFromFeed(
                    formattedLines,
                    line,
                    ref lineNumber,
                    firstOfferLine,
                    buffer,
                    isOnlyCategoriesNeed ) );
            
            ProcessFile( FilePath, func );
            
            return formattedLines.GetList();
        }
        
    }
}