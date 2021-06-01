// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using Admitad.Converters;
using Admitad.Converters.Workers;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Helpers;

using Messenger;

namespace AdmitadExamplesParser.Workers.Components
{
    internal sealed class Processor : BaseComponent
    {

        private readonly ProcessorSettings _settings;
        private List<List<string>> _ids = new ();
        private readonly DateTime _startTime;
        private readonly DbHelper _dbHelper;
        
        
        public Processor( ProcessorSettings settings, BackgroundBaseContext context )
            : base( ComponentType.Processor, context ) {
            _settings = settings;
            _startTime = DateTime.Now;
            _dbHelper = new DbHelper( SettingsBuilder.GetDbSettings() );
        }

        public void Start() {
            MeasureWorkTime( DoParseAndSave );
            
            var numberUnknownBrands = _dbHelper.GetUnknownBrandsCount();
            LogWriter.Log( $"{numberUnknownBrands} найдено новых брендов", true );
            _dbHelper.WriteUnknownBrands();
            
            PrintStatistics();
        }

        private IMessenger GetMessenger()
        {
            var messengerSettings = new MessengerSettings();
            foreach( var client in new IClientSettings[] { _settings.TelegramSettings } ) {
                messengerSettings.Clients.Add( client );
            }
            var messenger = new Messenger.Messenger( messengerSettings );
            return messenger;
        }
        
        private void PrintStatistics() {
            foreach( var line in StatisticsContainer.GetAllLines() ) {
                LogWriter.Log( line );
            }

            var blocks = new[] {
                ( "на скачивание фидов", ComponentType.Downloader ), ( "на индексирование", ComponentType.ElasticSearch ),
                ( "на привязку товаров", ComponentType.ProductLinker ), ( "всего затрачено времени", ComponentType.Processor )
            };
            foreach( var ( title, blockType ) in blocks ) {
                var downloadingStatistic = StatisticsContainer.GetSumBlockByName( blockType.ToString() );
                var time = TimeHelper.MillisecondsPretty( downloadingStatistic.WorkTime );
                LogWriter.Log( $"{ time } {title}", true);
            }

            var messenger = GetMessenger();
            LogWriter.WriteLog( messenger.Send );
        }

        private void DoParseAndSave()
        {
            FileSystemHelper.PrepareDirectory( _settings.DirectoryPath );
            DownloadFiles();
            
            LogWriter.Log( $"Начало: '{ _startTime }'" );
            
            var files = GetDownloadInfos();
            var documentsBefore =
                CreateElasticClient( _settings.ElasticSearchClientSettings ).GetCountAllDocuments();

            foreach( var fileInfo in files.Where( f => f.HasError == false ) ) {
                TryProcess( fileInfo );
            }
            
            LogWriter.Log( "Уснули на 15 сек", true );
            Thread.Sleep( 15000 );
            LogWriter.Log( "Продолжаем...", true );
            
            var documentsAfter = CreateElasticClient( _settings.ElasticSearchClientSettings ).GetCountAllDocuments();

            LogWriter.Log( $"{documentsAfter}/{documentsAfter - documentsBefore} всего товаров / новых товаров ", true );
            
            var linker = new ProductLinker( _settings.ElasticSearchClientSettings, _dbHelper, _context );
            linker.LinkCategories( _dbHelper.GetCategories() );
            linker.LinkTags( _dbHelper.GetTags() );

            var colors = _dbHelper.GetColors();
            var materials = _dbHelper.GetMaterials();
            var sizes = _dbHelper.GetSizes();
            
            linker.LinkProperties( colors, materials, sizes );
            linker.UnlinkProperties( colors, materials, sizes );
            linker.DisableProducts( _startTime );
            
            ResponseCollector.Responses.ForEach(
                r => {
                    LogWriter.Log(
                        $"{r.Item1}: {r.Item2}\n{r.Item3}",
                        r.Item3.ToString().ToLower().Contains( "invalid" ) );
                } );
            
            
        }

        private List<DownloadInfo> DownloadFiles() {
            var downloader = new FeedsDownloader( _settings.AttemptsToDownload, _dbHelper, _context );
            return downloader.DownloadsAll( _settings.DirectoryPath );
        }

        private List<DownloadInfo> GetDownloadInfos()
        {
            var fileInfos = Directory.GetFiles( _settings.DirectoryPath );
            return fileInfos.Where( f => f.Contains( "_" ) == false ).Select(
                f => new DownloadInfo( 0, Regex.Match( f, @".*\\(\w+)\.xml" ).Groups[ 1 ].Value ) {
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
                LogWriter.Log( $"{fileInfo.ShopName } error: {e.Message}", true );
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

        }

        private ShopData ParseFile( DownloadInfo fileInfo ) {
            var parser = new GeneralParser(
                fileInfo.FilePath,
                fileInfo.ShopName,
                _context,
                _settings.EnableExtendedStatistics );
            return parser.Parse();
        }

        private List<Product> ConvertToProducts( List<Offer> offers )
        {
            return new ProductConverter( _dbHelper ).GetProductsContainer( offers );
        }
        
        private List<Offer> CleanOffers(
            ShopData shopData ) {
            var converter = new OfferConverter( shopData, _dbHelper, _context );
            return converter.GetCleanOffers();
        }

        private void IndexProducts(
            IEnumerable<Product> products,
            ElasticSearchClientSettings settings )
        {
            var iProducts = products.Select( p => ( IProductForIndex ) p ).ToList();
            IndexEntities( iProducts, settings );
        }

        private void IndexEntities( 
            IEnumerable<IIndexedEntities> entities,
            ElasticSearchClientSettings settings )
        {
            var client = CreateElasticClient( settings );
            
            client.BulkAll( entities );
        }

        private ElasticSearchClient<IIndexedEntities> CreateElasticClient( ElasticSearchClientSettings settings )
        {
            return new ElasticSearchClient<IIndexedEntities>( settings, _context );
        }
        
    }
}