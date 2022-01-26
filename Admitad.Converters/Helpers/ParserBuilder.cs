// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using Admitad.Converters.Entities;
using Admitad.Converters.Workers;

using Common.Api;
using Common.Entities;

namespace Admitad.Converters.Helpers
{
    internal sealed class ParserBuilder
    {

        private readonly IMinimalDownloadsInfo _downloadsInfo;
        private readonly ParserType _type;
        private readonly BackgroundBaseContext _context;

        public ParserBuilder( IMinimalDownloadsInfo downloadsInfo, ParserType type, BackgroundBaseContext context ) =>
            ( _downloadsInfo, _type, _context ) = ( downloadsInfo, type, context );

        public List<IFeedParser> CreateParsers() =>
            _downloadsInfo.Files.Select( CreateParser ).ToList();

        private IFeedParser CreateParser( IFileInfo info )
        {
            var parserInfo = new ParsingInfo( info.FilePath, _downloadsInfo.ShopWeight, _downloadsInfo.NameLatin );
            return _type switch {
                ParserType.New => new FeedParser( parserInfo, _context ),
                ParserType.NewParallel => new ParallelFeedParser( parserInfo, _context ),
                _ => new GeneralParser( parserInfo, _context)
            };
        }
    }
}