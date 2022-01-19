// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

using Admitad.Converters.Entities;
using Admitad.Converters.OfferFilters;

using Common.Api;
using Common.Entities;
using Common.Helpers;

namespace Admitad.Converters.Workers
{
    public abstract class BaseFeedParser : BaseComponent, IFeedParser
    {

        #region Data
        
        protected readonly string FilePath;
        private int _offersTagCount = 0;
        private bool _categoryFilled = false;
        private readonly StringBuilder _offerBuffer = new();
        private int _brokenOffers = 0;
        private readonly string _shopNameLatin;
        protected readonly ShopData ShopData;
        private static int _count;
        private List<BrokenLine> BrokenLinesList => new();
        
        private readonly DateTime _updateTime;
        private IAvailabilityChecker _availabilityChecker;



        #region Regex
        
        private static readonly Regex ShopNameFromFilePath = new Regex(
            @"\\(?<shopName>\w+)\.xml",
            RegexOptions.Compiled );
        private static readonly Regex StartOffer = new( @"<offer[^s]", RegexOptions.Compiled );
        private static readonly Regex EndOffer = new( @"<\/offer>|deleted=""true"" id="".*""\/>", RegexOptions.Compiled );

        private static readonly Regex StartCategory = new( @"<categories>", RegexOptions.Compiled );
        private static readonly Regex EndCategory = new( @"<\/categories>", RegexOptions.Compiled );
        
        #endregion
        
        #endregion

        protected BaseFeedParser(
            IMinimalDownloadInfo downloadInfo,
            BackgroundBaseContext context )
            : base( ComponentType.GeneralParser, context ) =>
            ( FilePath, ShopData, _shopNameLatin, _updateTime ) =
            ( downloadInfo.FilePath, new ShopData( downloadInfo.ShopWeight ), downloadInfo.NameLatin, DateTime.Now );

        public ShopData Parse( bool isOnlyCategories = false ) {
            MeasureWorkTime( () => DoParse( isOnlyCategories ) );
            return ShopData;
        }

        private void DoParse( bool isOnlyCategories ) {
            Initialize();
            
            PrepareOffers(isOnlyCategories);
            
            ParseCategoriesAndFlushCategoriesCache();
            
            Context.AddMessage( $"Offers count {ShopData.NewOffers.Count}" );
        }
        
        protected abstract void PrepareOffers( bool isOnlyCategories );

        
        #region Feed lines processing

        protected bool ProcessLineFromFeed(
            ILinesBuffer formattedLines,
            string line,
            ref int lineCounter,
            int firstOfferLine,
            StringBuilder buffer,
            bool isOnlyCategoriesNeed ) {
            
            lineCounter++;
            FillCategoryBuffer( line );

            if( _categoryFilled && isOnlyCategoriesNeed ) {
                return true;
            }

            var startMatch = StartOffer.Match( line );
            
            if( lineCounter < firstOfferLine ) {
                if( startMatch.Success ) {
                    _offersTagCount++;
                }
                return false;
            }

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

        #endregion
        

        #region Offer processing
        
        protected void ProcessString( Func<RawOffer,ICollection<RawOffer>> collectorGetter, string line ) {
            _count++;
            var offerRaw = GetOfferRaw( line );
            if( IsNotAvailableOffer(offerRaw) ) {
                return;
            }

            AddOfferToData(collectorGetter,offerRaw);
        }

        private RawOffer GetOfferRaw( string offerLine ) {
            try {
                var rawOffer = Serialize<RawOffer>( offerLine );
                FillOffer( rawOffer );
                
                // rawOffer.Text = offerLine;
                
                return rawOffer;
            }
            catch( Exception e ) {
                AddStatisticLine( $"Bad line: {offerLine}" );
                BrokenLinesList.Add( new BrokenLine( e.Message, offerLine ) );
                
                return new RawOffer{ Text = offerLine };
            }
        }

        protected static int GetFirstOfferLine( string filePath )
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
        
        #endregion

        #region Routine
        private void Initialize() {
            TryGetShopData();
            SetAvailabilityChecker();
        }
        
        protected void ParseCategoriesAndFlushCategoriesCache()
        {
            var categoryRawString = _offerBuffer.ToString();
            _offerBuffer.Clear();
            
            var startMatch = StartCategory.Match( categoryRawString );
            var endMatch = EndCategory.Match( categoryRawString );
            if( startMatch.Success == false ||
                endMatch.Success == false ) {
                LogWriter.Log( $"{_shopNameLatin }: categories not found." );
                return;
            }
            
            var categoryString = categoryRawString.Substring( startMatch.Index, ( endMatch.Index + endMatch.Length ) - startMatch.Index );
            var categories = Serialize<ShopCategories>( categoryString );
            ShopData.AddCategories( categories.Categories );
        }
        
        protected static void ProcessFile( string filePath, Func<string, bool> action ) {
            var file = new StreamReader( filePath );
            string line;
            var sw = new Stopwatch();
            sw.Start();
            while( ( line = file.ReadLine() ) != null ) {
                var isBreak = action( line );
                if( isBreak ) {
                    break;
                }
            }
            file.Close();
            sw.Stop();
            Console.WriteLine($"Processing file time {sw.ElapsedMilliseconds}");
        }
        
        private void SetAvailabilityChecker() => _availabilityChecker = AvailabilityCheckersBuilder.GetChecker(ShopData.Name);
        
        private void TryGetShopData() {

            var m = ShopNameFromFilePath.Match( FilePath );
            if( m.Success == false ) {
                throw new ArgumentOutOfRangeException( "Not found shop info" );
            }

            ShopData.Name = m.Groups[ "shopName" ].Value;

        }

        private bool IsNotAvailableOffer(RawOffer offerRaw) => offerRaw == null || _availabilityChecker.IsOfferAvailable(offerRaw) == false;

        private static void AddOfferToData( Func<RawOffer,ICollection<RawOffer>> collectorGetter, RawOffer offerRaw ) {
            collectorGetter(offerRaw).Add(offerRaw);
        }

        private void FillOffer( RawOffer rawOffer ) {
            rawOffer.ShopName = ShopData.Name;
            rawOffer.UpdateTime = _updateTime;
            rawOffer.ShopNameLatin = _shopNameLatin;
            if( rawOffer.CategoryId != null ) {
                var categories = ShopData.GetCategories( rawOffer.CategoryId );
                rawOffer.CategoryPath = string.Join( " \\ ", categories.Select( c => c.Name ) );
                rawOffer.Categories = categories.Select( c => c.Id ).ToList();
                //rawOffer.CategoryPath = _shopData.GetBreadCrumbs( rawOffer.CategoryId );
            }
        }
        
        protected void AddStatistics( int formattedLinesCount, long getLinesTime, long serializeTime ) {
            AddStatisticLine( $"Shop name: {ShopData.Name}" );
            AddStatisticLine( $"File size: { new FileInfo( FilePath ).Length.ToString() }" );
            AddStatisticLine( $"Find offer entries count: { _offersTagCount }" );
            AddStatisticLine( $"Formatted lines count: { formattedLinesCount }", getLinesTime );
            AddStatisticLine( $"Find products count: { ShopData.NewOffers.Count }", serializeTime);
        }
        
        protected static T Serialize<T>( string rawEntity ) {
            var serializer = new XmlSerializer( typeof( T ) );
            using var reader = new StringReader( rawEntity );
            return ( T ) serializer.Deserialize( reader );
        }
        
        #endregion
    }
}