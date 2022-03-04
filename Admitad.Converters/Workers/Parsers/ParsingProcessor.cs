// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Admitad.Converters.Entities;
using Admitad.Converters.Helpers;

using Common.Api;
using Common.Entities;

namespace Admitad.Converters.Workers
{
    public sealed class ParsingProcessor
    {
        private readonly IMinimalDownloadsInfo _downloadsInfo;
        private readonly ParserType _type;
        private readonly BackgroundBaseContext _context;

        public ParsingProcessor( IMinimalDownloadsInfo downloadsInfo, ParserType type, BackgroundBaseContext context ) =>
            ( _downloadsInfo, _type, _context ) = ( downloadsInfo, type, context );

        public ShopData GetShopData()
        {
            var parsers = GetParsers();
            var datas = parsers.AsParallel().Select( GetShopData ).ToList();
            return MergeOffers( datas );
        }

        private static ShopData MergeOffers( List<ShopData> datas )
        {
            var primaryData = datas.First();
            foreach( var secondaryData in datas.Skip(1) ) {
                primaryData.InsertOffers( secondaryData );
            }

            return primaryData;
        }
        
        private List<IFeedParser> GetParsers()
        {
            var builder = new ParserBuilder( _downloadsInfo, _type, _context );
            return builder.CreateParsers();
        }

        private static ShopData GetShopData(
            IFeedParser parser ) =>
            parser.Parse( false );
    }
}