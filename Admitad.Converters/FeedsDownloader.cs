// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Admitad.Converters.Helpers;
using Admitad.Converters.Workers;

using AdmitadSqlData.Helpers;

using Common.Api;
using Common.Entities;
using Common.Helpers;

namespace Admitad.Converters
{
    public sealed class FeedsDownloader : BaseComponent
    {

        private readonly int _numberAttempts;
        //private readonly DbHelper _db;

        public event EventHandler<DownloadEventArgs> FileDownloaded;
        
        public FeedsDownloader(
            int numberAttempts,
            //DbHelper dbHelper,
            BackgroundBaseContext context  )
            : base( ComponentType.Downloader, context ) {
            _numberAttempts = Math.Max( numberAttempts, 1 );
            //_db = dbHelper;
        }

        public void DownloadsAll( string directoryPath, List<ShopInfo> infos )
        {
            MeasureWorkTime( () => DoDownloadAll( directoryPath, infos ) );
        }

        public DownloadsInfo Download( string directoryPath, ShopInfo info )
        {
            return DoDownload( new List<ShopInfo> {info}, directoryPath ).First();
        }

        private List<DownloadsInfo> DoDownloadAll( string directoryPath, List<ShopInfo> infos )
        {
            return DoDownload( infos, directoryPath );
        }

        private List<DownloadsInfo> DoDownload( IReadOnlyCollection<ShopInfo> files, string directoryPath )
        {
            Context.TotalActions = files.Sum( f => f.Feeds.Count );
            var downloadInfos = files.AsParallel().Select( f => DownloadFeeds( f, directoryPath ) ).ToList();
            var withErrors = downloadInfos.Where( i => i.HasErrors ).ToList();
            if( withErrors.Any() ) {
                TryAgainIfNeed( withErrors );
            }
            
            var fileCount = Directory.GetFiles( directoryPath ).Length;
            LogWriter.Log( $"{fileCount} удалось скачать из {files.Count}", true );
            Context.AddMessage( $"Всего {downloadInfos.Count} c ошибками {withErrors.Count}" );
            return downloadInfos;
        }
        
        private static List<DownloadsInfo> GetNeedDownload( IEnumerable<DownloadsInfo> infos ) =>
             infos.Where( i => i.FeedsInfos.Any( fi => fi.NeedDownload) ).ToList();

        private void TryAgainIfNeed( List<DownloadsInfo> infos )
        {
            var needDownload = GetNeedDownload( infos );
            
            if( needDownload.Any() == false ) {
                return;
            }

            for( var i = 1; i <= _numberAttempts; i++ ) {
                LogWriter.Log( $"{i}/{_numberAttempts} попытка докачать фиды открытых магазинов.", true );
                var downloaded = needDownload.AsParallel().Select( DoDownloadFiles ).ToList();
                needDownload = GetNeedDownload( downloaded );
                if( needDownload.Any() == false ) {
                    LogWriter.Log( "All have finished.", false );
                    break;
                }
            }

            if( needDownload.Any() ) {
                LogWriter.Log( 
                    $"Failed to download: {string.Join(",",needDownload.Select( i => i.ShopName ))}",
                    true );
            }
        }

        private DownloadsInfo DownloadFeeds(
            ShopInfo shopInfo,
            string directoryPath ) {
            
            FileWork( directoryPath, shopInfo );
            
            var downloadInfo = new DownloadsInfo( shopInfo ) {
                StartTime = DateTime.Now,
                ShopName = shopInfo.Name,
            };

            FillFilePaths( downloadInfo, shopInfo, directoryPath );
            
            return DoDownloadFiles( downloadInfo );
        }

        private static void FillFilePaths( DownloadsInfo info, ShopInfo shopInfo, string directoryPath ) {
            foreach( var feedInfo in info.FeedsInfos ) {
                feedInfo.FilePath = FilePathHelper.GetFilePath( directoryPath, feedInfo.Id, shopInfo );
            }
        }
        
        private static void FileWork( string directoryPath, ShopInfo shopInfo ) {
            
            foreach( var feedInfo in shopInfo.Feeds ) {
                var filePath = FilePathHelper.GetFilePath( directoryPath, feedInfo.Id, shopInfo );
                var oldDirectoryPath = FilePathHelper.CombinePath( directoryPath, "old/" );
            
                if( File.Exists( filePath ) == false ) {
                    return;
                }
            
                if( Directory.Exists( oldDirectoryPath ) == false ) {
                    Directory.CreateDirectory( oldDirectoryPath );
                }
            
                var oldFilePath = FilePathHelper.GetFilePath( oldDirectoryPath, feedInfo.Id, shopInfo );
            
                if( File.Exists( oldFilePath ) ) {
                    File.Delete( oldFilePath );
                }
            
                File.Move( filePath, oldFilePath );
            }
        }
        
        private DownloadsInfo DoDownloadFiles( DownloadsInfo downloadsInfo ) {
            var lastUpdateDate = downloadsInfo.LastUpdate.ToString( "yyyy.MM.dd.HH.mm" );

            var tasks = downloadsInfo.FeedsInfos.Where( fi => fi.NeedDownload ).Select(
                fi => Task.Factory.StartNew( () => DoDownloadFile( fi, lastUpdateDate, downloadsInfo ) ) )
                .ToArray();
            
            // foreach( var feedInfo in downloadsInfo.FeedsInfos.Where( fi => fi.NeedDownload ) ) {
            //     DoDownloadFile( feedInfo, lastUpdateDate, downloadsInfo );
            // }

            Task.WaitAll( tasks );

            if( downloadsInfo.HasErrors == false ) {
                Context.AddMessage( $"скачали { downloadsInfo.ShopName }. Фидов: {downloadsInfo.FeedsInfos.Count}" );
                Context.CalculatePercent();
                
                FileDownloaded?.Invoke( this, new DownloadEventArgs( downloadsInfo ) );
                
            }
            
            return downloadsInfo;
        }

        private void DoDownloadFile( FeedInfo info, string lastUpdateDate, DownloadsInfo downloadsInfo ) {
            
            var url = downloadsInfo.VersionProcessing == 2
                ? $"{info.Url}&last_import={lastUpdateDate}"
                : info.Url;
            
            try {
                Measure(
                    () => {
                        var webClient = new WebClient();
                        webClient.DownloadFile( url, info.FilePath );
                    },
                    out var workTime );

                if( FileChecker.WithAnEnd( info.FilePath ) == false ) {
                    if( File.Exists( info.FilePath ) ) {
                        File.Delete( info.FilePath );
                    }
                    info.Error = DownloadError.Unfinished;
                    Context.AddMessage( $"{ downloadsInfo.ShopName }: { DownloadError.Unfinished }", true );
                    LogWriter.Log( $"{downloadsInfo.ShopName} ошибка { info.Error }", true );
                    return;
                }
                
                info.Error = DownloadError.Ok;
                info.FileSize = new FileInfo( info.FilePath ).Length;
                info.DownloadTime = workTime;
                LogWriter.Log( $"Downloaded {downloadsInfo.ShopName} feed with id {info.Id}, time {workTime} " );
            }
            catch( Exception e ) {
                info.Error = GetError( e.Message );
                var errorMessage =
                    info.Error == DownloadError.UnknownError
                        ? e.Message
                        : info.Error.ToString();
                Context.AddMessage( $"{ downloadsInfo.ShopName }: { errorMessage }", true );
                LogWriter.Log( $"{downloadsInfo.ShopName} ошибка { errorMessage }", true );
            }
        }
        
        private static DownloadError GetError( string message ) =>
            message switch {
                "The remote server returned an error: (429) Unknown Status Code." => DownloadError.ManyRequests,
                "The remote server returned an error: (400) Bad Request." => DownloadError.ClosedStore,
                _ => DownloadError.UnknownError
            };
    }
}