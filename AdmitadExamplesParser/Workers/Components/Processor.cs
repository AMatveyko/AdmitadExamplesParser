// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Admitad.Converters;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadExamplesParser.Entities;

namespace AdmitadExamplesParser.Workers.Components
{
    internal sealed class Processor : BaseComponent
    {

        private readonly ProcessorSettings _settings;
        private List<List<string>> _ids = new ();
        private readonly DateTime _startTime;
        
        
        public Processor( ProcessorSettings settings ) : base( ComponentType.Processor ) {
            _settings = settings;
            _startTime = DateTime.Now;
        }

        public void Start() {
            MeasureWorkTime( DoParseAndSave );
            PrintStatistics();
            
            LogWriter.WriteLog();
        }

        private static void PrintStatistics() {
            foreach( var line in StatisticsContainer.GetAllLines() ) {
                LogWriter.Log( line );
            }

            //Console.ReadLine();
        }

        private void DoParseAndSave()
        {
            FileSystemHelper.PrepareDirectory( _settings.DirectoryPath );
            DownloadFiles();
            
            LogWriter.Log( $"Start: '{ _startTime }'" );
            
            var files = GetDownloadInfos();
            foreach( var fileInfo in files.Where( f => f.HasError == false ) ) {
                TryProcess( fileInfo );
            }

            var linker = new ProductLinker( _settings.ElasticSearchClientSettings );
            linker.CategoryLink();
            linker.TagsLink();
            
            linker.LinkProperties();
            
            linker.UnlinkProperties();
            
            linker.DisableProducts( _startTime );
            
            ResponseCollector.Responses.ForEach( r => LogWriter.Log( $"{r.Item1}: {r.Item2}\n{r.Item3}" ) );
            
        }

        private List<DownloadInfo> DownloadFiles() {
            var downloader = new FeedsDownloader();
            return downloader.DownloadsAll( _settings.DirectoryPath );
        }

        private List<DownloadInfo> GetDownloadInfos()
        {
            var fileInfos = Directory.GetFiles( _settings.DirectoryPath );
            return fileInfos.Where( f => f.Contains( "_" ) == false ).Select(
                f => new DownloadInfo {
                    DownloadTime = 0,
                    FilePath = f,
                    ShopName = Regex.Match( f, @".*\\(\w+)\.xml" ).Groups[ 1 ].Value
                } ).ToList();
        }

        private void TryProcess(
            DownloadInfo fileInfo )
        {
            try { DoProcess( fileInfo ); }
            catch( Exception e ) {
                LogWriter.Log( $"{fileInfo.ShopName } error: {e.Message}" );
            }
        }
        
        private void DoProcess( DownloadInfo fileInfo )
        {
            LogWriter.Log( fileInfo.ShopName );
            var shopData = ParseFile( fileInfo );
            LogWriter.Log( $"NullOffers: {shopData.Offers.Count( o => o == null )}" );
            if( shopData.CategoryLoop ) {
                LogWriter.Log( "Category loop!" );
            }
            var offers = CleanOffers( shopData );
            var products = ConvertToProducts( offers );

            _settings.ElasticSearchClientSettings.ShopName = shopData.Name;

            IndexProducts( products, _settings.ElasticSearchClientSettings );
            var client = new ElasticSearchClient<IIndexedEntities>( _settings.ElasticSearchClientSettings );
            //client.BulkLinkedData( LinkedDataContainer.Container );

            //IndexProducts( products, settings );
            //IndexOffers( offers, _settings.ElasticSearchClientSettings );
            //_ids.Add( products.Select( p => p.Id ).ToList() );
        }

        private ShopData ParseFile( DownloadInfo fileInfo ) {
            var parser = new GeneralParser( fileInfo.FilePath, fileInfo.ShopName, _settings.EnableExtendedStatistics );
            return parser.Parse();
        }

        private static List<Product> ConvertToProducts( List<Offer> offers )
        {
            return ProductConverter.GetProductsContainer( offers );
        }
        
        private static List<Offer> CleanOffers(
            ShopData shopData ) {
            var converter = new OfferConverter( shopData );
            return converter.GetCleanOffers();
        }

        private static void IndexOffers(
            IEnumerable<Offer> offers,
            ElasticSearchClientSettings settings )
        {
            settings.DefaultIndex = Offer.IndexNameConst;
            IndexEntities( offers, settings );
        }

        private static void IndexProducts(
            IEnumerable<Product> products,
            ElasticSearchClientSettings settings )
        {
            settings.DefaultIndex = Product.IndexName;
            var iProducts = products.Select( p => ( IProductForIndex ) p ).ToList();
            IndexEntities( iProducts, settings );
        }

        private static void IndexEntities( 
            IEnumerable<IIndexedEntities> entities,
            ElasticSearchClientSettings settings )
        {
            var client = new ElasticSearchClient<IIndexedEntities>( settings );
            
            client.BulkAll( entities );
        }
        
        // private static void Serialize( IEnumerable<Offer> offers, string fileName = "" ) {
        //     var x = new System.Xml.Serialization.XmlSerializer( typeof( List<Offer> ) );
        //     var t = new StreamWriter(  fileName );
        //     x.Serialize( t, offers.ToList() );
        //     t.Close();
        // }
    }
}