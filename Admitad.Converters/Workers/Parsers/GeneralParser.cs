// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Admitad.Converters.OfferFilters;
using Common.Api;
using Common.Entities;
using Common.Helpers;

namespace Admitad.Converters.Workers
{
    public sealed class GeneralParser : BaseComponent, IFeedParser
    {


        private int _offersTagCount = 0;
        
        private const string OfferGroupName = "offer";
        private const string ShopGroupName = "shopName";
        private static readonly Regex OfferPattern = new(
            "(.+)?(?<" + OfferGroupName + @"><offer.+\/offer>)",
            RegexOptions.Compiled );

        private static readonly Regex ShopIdFromFilePathPattern = new(
            @"\\(?<shopId>\d+)_\d+\.xml",
            RegexOptions.Compiled );
        
        private static readonly Regex ShopPattern = new(
            @"<shop>(\s+)?(<name>|<title>)(?<" + ShopGroupName + @">(.[^<>]+))(<\/name>|<\/title>)",
            RegexOptions.Compiled );

        private static readonly Regex ShopNameFromFilePath = new Regex(
            @"\\(?<shopName>\w+)\.xml",
            RegexOptions.Compiled );
        
        private static readonly Regex AllOffersPattern = new(
            @"(?<offers><offers><offer.*<\/offer>(?<endOffers><\/offers>)?)",
            RegexOptions.Compiled );

        private static readonly Regex StartOffer = new( @"<offer[^s]", RegexOptions.Compiled );
        private static readonly Regex EndOffer = new( @"<\/offer>|deleted=""true"" id="".*""\/>", RegexOptions.Compiled );

        private static readonly Regex StartCategory = new( @"<categories>", RegexOptions.Compiled );
        private static readonly Regex EndCategory = new( @"<\/categories>", RegexOptions.Compiled );
        
        private readonly ShopData _shopData;
        private const int FindShopDataDeep = 10;

        private int _brokenOffers = 0;
        private List<string> _brokenLines = new();


        private readonly StringBuilder _offerBuffer = new();
        private StringBuilder _categoryBuffer = new();
        private List<string> _offerLines = new();

        private readonly string _filePath;
        private readonly bool _enableExtendedStatistic;

        private readonly DateTime _updateTime;
        private readonly string _shopNameLatin;
        private IAvailabilityChecker _availabilityChecker;

        private bool _categoryFilled = false;

        private List<BrokenLine> BrokenLinesList => new();
        private static int Count { get; set; }

        public GeneralParser(
            //string filePath,
            //string shopNameLatin,
            IMinimalDownloadInfo downloadInfo,
            BackgroundBaseContext context ,
            bool enableExtendedStatistic = false ) : base( ComponentType.GeneralParser, context )
        {
            //_filePath = filePath;
            _filePath = downloadInfo.FilePath;
            _enableExtendedStatistic = enableExtendedStatistic;
            _shopData = new ShopData( downloadInfo.ShopWeight );
            //_shopNameLatin = shopNameLatin;
            _shopNameLatin = downloadInfo.NameLatin;
            _updateTime = DateTime.Now;
        }

        public ShopData Parse( bool isOnlyCategories = false) {
            MeasureWorkTime( () => DoParse(isOnlyCategories) );
            return _shopData;
        }

        private void DoParse(bool isOnlyCategories)
        {
            TryGetShopData();
            SetAvailabilityChecker();
            var formattedLines = Measure( () => GetFormattedOfferLinesWithProcessFile(isOnlyCategories), out var getLinesTime );
            
            ParseCategories();
            
            Measure( () => ParseOffer( formattedLines ), out var serializeTime );
            AddStatistics( formattedLines.Count, getLinesTime, serializeTime );
            Context.AddMessage( $"Offers count {_shopData.NewOffers.Count}" );
            
        }

        private void ParseCategories()
        {
            var categoryRawString = _offerBuffer.ToString();
            var startMatch = StartCategory.Match( categoryRawString );
            var endMatch = EndCategory.Match( categoryRawString );
            if( startMatch.Success == false ||
                endMatch.Success == false ) {
                LogWriter.Log( $"{_shopNameLatin }: categories not found." );
                return;
            }
            
            var categoryString = categoryRawString.Substring( startMatch.Index, ( endMatch.Index + endMatch.Length ) - startMatch.Index );
            var categories = Serialize<ShopCategories>( categoryString );
            _shopData.AddCategories( categories.Categories );
        }
        
        private void AddStatistics( int formattedLinesCount, long getLinesTime, long serializeTime ) {
            AddStatisticLine( $"Shop name: {_shopData.Name}" );
            AddStatisticLine( $"File size: { new FileInfo( _filePath ).Length.ToString() }" );
            AddStatisticLine( $"Find offer entries count: { _offersTagCount }" );
            AddStatisticLine( $"Formatted lines count: { formattedLinesCount }", getLinesTime );
            AddStatisticLine( $"Find products count: { _shopData.NewOffers.Count }", serializeTime);
        }

        private void TryGetShopData()
        {

            var m = ShopNameFromFilePath.Match( _filePath );
            if( m.Success == false ) {
                throw new ArgumentOutOfRangeException( "Not found shop info" );
            }

            _shopData.Name = m.Groups[ "shopName" ].Value;

        }

        private void SetAvailabilityChecker() => _availabilityChecker = AvailabilityCheckersBuilder.GetChecker(_shopData.Name);

        private void ParseOffer( List<string> lines ) {
            lines.ForEach( ProcessString );
        }

        private static void ProcessFile(
            string filePath, Func<string, bool> action )
        {
            var file = new StreamReader( filePath );
            string line;
            while( ( line = file.ReadLine() ) != null ) {
                var isBreak = action( line );
                if( isBreak ) {
                    break;
                }
            }
            file.Close();
        }

        private void FillCategoryBuffer( string line ) {
            if( _categoryFilled ) {
                return;
            }

            _offerBuffer.Append( line );
            var m = EndCategory.Match( line );
            if( m.Success ) {
                _categoryFilled = true;
            }

        }

        private List<string> GetFormattedOfferLinesWithProcessFile(bool isOnlyCategoriesNeed) {
            var formattedLines = new List<string>();
            var buffer = new StringBuilder();
            var firstOfferLine = GetFirstOfferLine( _filePath );
            var lineNumber = 0;
            
            var func = new Func<string, bool>(
                line => {
                    lineNumber++;
                    FillCategoryBuffer( line );

                    if( _categoryFilled && isOnlyCategoriesNeed ) {
                        return true;
                    }

                    if( lineNumber < firstOfferLine ) {
                        if( StartOffer.Match( line ).Success ) {
                            _offersTagCount++;
                        }
                        return false;
                    }
                
                    var startMatch = StartOffer.Match( line );
                    var endMatch = EndOffer.Match( line );

                    if( startMatch.Success ) {
                        _offersTagCount++;
                        if( buffer.Length > 0 ) {
                            buffer.Clear();
                            _brokenOffers++;
                        }
                        if( endMatch.Success ) {
                            var subLine = startMatch.Index > 0 ? line.Substring( startMatch.Index ) : line;
                            formattedLines.Add( subLine );
                            return false;
                        }

                        buffer.Append( line );
                        return false;
                    }

                    if( endMatch.Success ) {
                        buffer.Append( line );
                        var newLine = buffer.ToString();
                        var newStartMatch = StartOffer.Match( newLine );
                        var subLine = newStartMatch.Index > 0 ? newLine.Substring( newStartMatch.Index ) : newLine;
                        formattedLines.Add( subLine );
                        buffer.Clear();
                    }

                    buffer.Append( line );
                    
                    return false;
                });
            
            ProcessFile( _filePath, func );
            
            return formattedLines;
        }
        
        private static int GetFirstOfferLine( string filePath )
        {
            var lineNumber = 0;
            var func = new Func<string, bool>(
                line => {
                    lineNumber++;
                    var m = StartOffer.Match( line );
                    return m.Success;
                });
            
            ProcessFile( filePath, func );
            
            return lineNumber;
        }

        private void ProcessString( string line ) {
            Count++;
            var offerRaw = GetOfferRaw( line );
            if( IsNotAvailableOffer(offerRaw) ) {
                return;
            }

            AddOfferToData(offerRaw);
        }

        private bool IsNotAvailableOffer(RawOffer offerRaw) => offerRaw == null || _availabilityChecker.IsOfferAvailable(offerRaw) == false;

        private void AddOfferToData( RawOffer offerRaw) {
            var collector = offerRaw.IsDeleted
                ? _shopData.DeletedOffers
                : _shopData.NewOffers;

            collector.Add(offerRaw);
        }

        private RawOffer GetOfferRaw(
            string offerLine )
        {
            try {
                var rawOffer = Serialize<RawOffer>( offerLine );
                FillOffer( rawOffer );
                
                rawOffer.Text = offerLine;
                
                return rawOffer;
            }
            catch( Exception e ) {
                AddStatisticLine( $"Bad line: {offerLine}" );
                BrokenLinesList.Add( new BrokenLine( e.Message, offerLine ) );
                
                return new RawOffer{ Text = offerLine };
            }
        }

        private void FillOffer( RawOffer rawOffer ) {
            rawOffer.ShopName = _shopData.Name;
            rawOffer.UpdateTime = _updateTime;
            rawOffer.ShopNameLatin = _shopNameLatin;
            if( rawOffer.CategoryId != null ) {
                var categories = _shopData.GetCategories( rawOffer.CategoryId );
                rawOffer.CategoryPath = string.Join( " \\ ", categories.Select( c => c.Name ) );
                rawOffer.Categories = categories.Select( c => c.Id ).ToList();
                //rawOffer.CategoryPath = _shopData.GetBreadCrumbs( rawOffer.CategoryId );
            }
        }
        
        private static T Serialize<T>( string rawEntity ) {
            var serializer = new XmlSerializer( typeof( T ) );
            using var reader = new StringReader( rawEntity );
            return ( T ) serializer.Deserialize( reader );
        }
    }
}