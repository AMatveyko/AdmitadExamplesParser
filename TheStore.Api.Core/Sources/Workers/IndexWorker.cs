// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Admitad.Converters;

using AdmitadCommon.Entities;
using AdmitadCommon.Entities.Api;
using AdmitadCommon.Helpers;

using AdmitadSqlData.Helpers;

using Common.Settings;

using Newtonsoft.Json;

using NLog;

namespace TheStore.Api.Core.Sources.Workers
{
    internal sealed class IndexWorker : BaseLinkWorker
    {

        private static readonly Logger Logger = LogManager.GetLogger( "IndexLogger" );
        
        private readonly ProcessorSettings _settings;
        private readonly ParallelBackgroundContext _context;

        public IndexWorker(
            ProcessorSettings settings,
            ParallelBackgroundContext context,
            BackgroundWorks works,
            DbHelper dbHelper )
            : base( settings.ElasticSearchClientSettings, works, dbHelper )
        {
            _settings = settings;
            _context = context;
        }

        public void LinkAll( LinkAllContext context )
        {
            CheckContextType( context );
            DoLink();
            Wait();
            LogResult();
        }

        private void CheckContextType< T >( T context )
        {
            if( ReferenceEquals( context, _context ) == false ) {
                throw new ArgumentException( "Разные контексты!" );
            }
        }
        
        public void Index( IndexShopContext context )
        {
            
            CheckContextType( context );
            
            var xmlInfo = Db.GetShop( context.ShopId );
            context.ShopName = xmlInfo.Name;

            var file = DoDownloadIfNeed( context, xmlInfo );
            context.SetProgress( 30, 100 );
            if( file.HasError ) {
                
                context.Finish();
                context.IsError = true;
                
                return;
            }
            
            DoIndexShop( xmlInfo.ShopId, file, true, context.NeedSoldOut );
            context.SetProgress( 60, 100 );
            if( context.NeedLink ) {
                DoLink();
                context.SetProgress( 90, 100 );
            }
            Wait();
            LogResult();
        }

        public void IndexAll( IndexAllShopsContext context )
        {
            
            CheckContextType( context );
            
            var xmlInfos = Db.GetEnableShops();
            DownloadAll( xmlInfos );
            context.SetProgress( 60, 100 );
            DoLink();
            context.SetProgress( 100, 100 );
            Wait();
            FillAddDate();
            DbWork( context );
            LogResult();
        }
        
        private void DbWork( IndexAllShopsContext context )
        {
            Db.WriteUnknownBrands();
            Db.SaveUnknownCountries();
        }
        
        private void DownloadAll( List<XmlFileInfo> infos )
        {
            var downloadContext = new BackgroundBaseContext( "Download:All", "download" );
            downloadContext.Prepare();
            _context.AddContext( downloadContext );
            
            var downloader = new FeedsDownloader( _settings.AttemptsToDownload, Db, downloadContext );

            downloader.FileDownloaded += HandleDownloadEvent;
            
            downloadContext.Start();
            
            downloader.DownloadsAll( _settings.DirectoryPath, infos );
            
            downloadContext.Content = "Все скачали";
            downloadContext.Finish();

        }

        private void HandleDownloadEvent( object sender, DownloadEventArgs e )
        {
            DoIndexShop( e.Info.ShopId, e.Info, false, true );
        }

        private void Wait()
        {
            while( _context.Contexts.Any( c => c.IsFinished == false ) ) {
                Thread.Sleep( 30000 );
            }
        }
        
        private void DoIndexShop( int shopId, DownloadInfo fileInfo, bool single, bool needSoldOut )
        {
            var processShopContext = new ProcessShopContext(
                $"{shopId}:{(single ? "single" : "all")}",
                shopId,
                fileInfo,
                needSoldOut );
            _context.AddContext( processShopContext );
            Works.AddToQueue( UpdateShop, processShopContext, QueuePriority.Medium, false );
        }

        private void UpdateShop( ProcessShopContext context )
        {
            var shopHandler = new ShopHandler( context, _settings, Db );
            shopHandler.Process();
        }
        
        private void DoLink()
        {
            LinkCountries();
            LinkProperties();
            UnlinkProperties();
            LinkCategories();
            LinkTags();
        }

        private void FillAddDate()
        {
            var linker = new ProductLinker( _settings.ElasticSearchClientSettings, Db, _context );
            var result = linker.FillAddDate();
            _context.AddMessage( $"addDate: updated {result.Pretty}" );
        }
        
        private void LinkCountries()
        {
            var linkContext = new CountriesLinkContext( _context.Id );
            _context.AddContext( linkContext );
            var worker = new CountryWorker( _settings.ElasticSearchClientSettings, Works, Db );
            Works.AddToQueue( worker.LinkAll, linkContext, QueuePriority.Medium, false );
        }
        
        private void LinkTags()
        {
            var linkContext = new LinkTagsContext( _context.Id );
            _context.AddContext( linkContext );
            var worker = new TagsWorker( _settings.ElasticSearchClientSettings, Works, Db );
            
            Works.AddToQueue( worker.LinkTags, linkContext, QueuePriority.Medium, false );
        }
        
        private void LinkCategories()
        {
            var linkContext = new LinkCategoriesContext( _context.Id );
            _context.AddContext( linkContext );
            var worker = new CategoryWorker( _settings.ElasticSearchClientSettings, Works, Db );
            
            Works.AddToQueue( worker.LinkCategories, linkContext, QueuePriority.Medium, false );
        }

        private void UnlinkProperties()
        {
            var linkContext = new UnlinkPropertiesContext( _context.Id );
            _context.AddContext( linkContext );
            var worker = new PropertiesWorker( _settings.ElasticSearchClientSettings, Works, Db );
            
            Works.AddToQueue( worker.UnlinkProperties, linkContext, QueuePriority.Medium, false );
        }
        
        private void LinkProperties()
        {
            var linkContext = new LinkPropertiesContext( _context.Id );
            _context.AddContext( linkContext );
            var worker = new PropertiesWorker( _settings.ElasticSearchClientSettings, Works, Db );
            
            Works.AddToQueue( worker.LinkProperties, linkContext, QueuePriority.Medium, false );
        }

        private DownloadInfo DoDownloadIfNeed( IndexShopContext context, XmlFileInfo xmlInfo )
        {
            var filePath = FilePathHelper.GetFilePath( _settings.DirectoryPath, xmlInfo );
            if( context.DownloadFresh ||
                File.Exists( filePath ) == false ) {
                var downloadContext = new BackgroundBaseContext($"Download:{xmlInfo.Name}", "download");
                downloadContext.Prepare();
                downloadContext.Start();
                
                context.Contexts.Add( downloadContext );
                
                var downloader = new FeedsDownloader( _settings.AttemptsToDownload, Db, downloadContext );
                var file = downloader.Download( xmlInfo, _settings.DirectoryPath );

                downloadContext.Content = "Все скачали";
                downloadContext.Finish();

                return file;
            }

            return new DownloadInfo( xmlInfo.ShopId, xmlInfo.NameLatin, xmlInfo.Weight ) {
                ShopName = xmlInfo.NameLatin,
                FilePath = filePath
            };
        }

        private void LogResult()
        {
            var result = JsonConvert.SerializeObject( _context );
            Logger.Info( result );
        }

    }
}