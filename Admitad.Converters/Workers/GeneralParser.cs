// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Helpers;

namespace Admitad.Converters.Workers
{
    public sealed class GeneralParser : BaseComponent
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
        private static readonly Regex EndOffer = new( @"<\/offer>", RegexOptions.Compiled );

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

        private bool _categoryFilled = false;

        private List<BrokenLine> BrokenLinesList => new();
        private static int Count { get; set; }

        public GeneralParser(
            string filePath,
            string shopNameLatin,
            BackgroundBaseContext context ,
            bool enableExtendedStatistic = false ) : base( ComponentType.GeneralParser, context )
        {
            _filePath = filePath;
            _enableExtendedStatistic = enableExtendedStatistic;
            _shopData = new ShopData();
            _shopNameLatin = shopNameLatin;
            _updateTime = DateTime.Now;
        }

        public ShopData Parse() {
            MeasureWorkTime( DoParse );
            return _shopData;
        }

        private void DoParse()
        {
            TryGetShopData();
            var formattedLines = Measure( GetFormattedOfferLinesWithProcessFile , out var getLinesTime );
            ParseCategories();
            Measure( () => ParseOffer( formattedLines ), out var serializeTime );
            AddStatistics( formattedLines.Count, getLinesTime, serializeTime );
            _context.AddMessage( $"Offers count {_shopData.Offers.Count}" );
            
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
            AddStatisticLine( $"Find products count: { _shopData.Offers.Count }", serializeTime);
            // if( _enableExtendedStatistic ) {
            //     Measure(
            //         () =>
            //             ExtendedStatistics.AddStatisticsForParsing(
            //                 _shopData, line => AddStatisticLine( line ) ),
            //         out var extendedStatisticsTime );
            //     AddStatisticLine( $"Extended statistics execution time ", extendedStatisticsTime );
            // }
        }

        private void TryGetShopData()
        {

            var m = ShopNameFromFilePath.Match( _filePath );
            if( m.Success == false ) {
                throw new ArgumentOutOfRangeException( "Not found shop info" );
            }

            _shopData.Name = m.Groups[ "shopName" ].Value;

            // oldLogic
            // var lineNumber = 0;
            // var func = new Func<string, bool>(
            //     line => {
            //         lineNumber++;
            //         if( lineNumber > FindShopDataDeep ) {
            //             AddStatisticLine( _filePath );
            //             AddStatisticLine( "Not found shop info" );
            //             throw new ArgumentOutOfRangeException( "Not found shop info" );
            //         }
            //
            //         var m = ShopPattern.Match( line );
            //         if( m.Success ) {
            //             _shopData.Name = m.Groups[ ShopGroupName ].Value;
            //             return true;
            //         }
            //
            //         return false;
            //     });
            //
            // ProcessFile( _filePath, func );

        }

        private void ParseOffer( List<string> lines ) {
            //lines.AsParallel().ForAll( ProcessString );
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

        private List<string> GetFormattedOfferLinesWithProcessFile() {
            var formattedLines = new List<string>();
            var buffer = new StringBuilder();
            var firstOfferLine = GetFirstOfferLine( _filePath );
            var lineNumber = 0;
            
            var func = new Func<string, bool>(
                line => {
                    lineNumber++;
                    FillCategoryBuffer( line );
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
            if( offerRaw == null ) {
                return;
            }
            
            _shopData.Offers.Add( offerRaw );
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
                rawOffer.CategoryPath = _shopData.GetCategoryPath( rawOffer.CategoryId );
            }
        }
        
        private static T Serialize<T>( string rawEntity ) {
            var serializer = new XmlSerializer( typeof( T ) );
            using var reader = new StringReader( rawEntity );
            return ( T ) serializer.Deserialize( reader );
        }
    }
}