// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

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
        private readonly DbHelper _db;

        public event EventHandler<DownloadEventArgs> FileDownloaded;
        
        public FeedsDownloader(
            int numberAttempts,
            DbHelper dbHelper,
            BackgroundBaseContext context  )
            : base( ComponentType.Downloader, context ) {
            _numberAttempts = Math.Max( numberAttempts, 1 );
            _db = dbHelper;
        }

        public void DownloadsAll( string filePath, List<XmlFileInfo> infos )
        {
            MeasureWorkTime( () => DoDownloadAll( filePath, infos ) );
        }

        public DownloadInfo Download( string filePath, XmlFileInfo info )
        {
            return DoDownload( new List<XmlFileInfo> {info}, filePath ).First();
        }

        private List<DownloadInfo> DoDownloadAll( string filePath, List<XmlFileInfo> infos )
        {
            return DoDownload( infos, filePath );
        }

        private List<DownloadInfo> DoDownload( IReadOnlyCollection<XmlFileInfo> files, string filePath )
        {
            _context.TotalActions = files.Count;
            var downloadInfos = files.AsParallel().Select( f => DownloadFile( f, filePath ) ).ToList();
            var withErrors = downloadInfos.Where( i => i.HasError ).ToList();
            if( withErrors.Any() ) {
                TryAgainIfNeed( withErrors );
            }
            
            var fileCount = Directory.GetFiles( filePath ).Length;
            LogWriter.Log( $"{fileCount} удалось скачать из {files.Count}", true );
            _context.AddMessage( $"Всего {downloadInfos.Count} c ошибками {withErrors.Count}" );
            return downloadInfos;
        }
        
        private static List<DownloadInfo> GetNeedDownload( IEnumerable<DownloadInfo> infos ) =>
            infos.Where( i => i.Error != DownloadError.Ok && i.Error != DownloadError.ClosedStore ).ToList();
        
        private void TryAgainIfNeed( List<DownloadInfo> infos )
        {
            var needDownload = GetNeedDownload( infos );
            
            if( needDownload.Any() == false ) {
                return;
            }

            for( var i = 1; i <= _numberAttempts; i++ ) {
                LogWriter.Log( $"{i}/{_numberAttempts} попытка докачать фиды открытых магазинов.", true );
                var downloaded = needDownload.AsParallel().Select( DoDownloadFile ).ToList();
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

        private DownloadInfo DownloadFile(
            XmlFileInfo fileInfo,
            string directoryPath ) {
            
            FileWork( directoryPath, fileInfo );
            
            var downloadInfo = new DownloadInfo( fileInfo ) {
                StartTime = DateTime.Now,
                ShopName = fileInfo.Name,
                Url = fileInfo.XmlFeed,
                FilePath = FilePathHelper.GetFilePath( directoryPath, fileInfo )
            };

            return DoDownloadFile( downloadInfo );
        }

        private static void FileWork( string directoryPath, XmlFileInfo fileInfo )
        {
            var filePath = FilePathHelper.GetFilePath( directoryPath, fileInfo );
            var oldDirectoryPath = FilePathHelper.CombinePath( directoryPath, "old/" );
            
            if( File.Exists( filePath ) == false ) {
                return;
            }
            
            if( Directory.Exists( oldDirectoryPath ) == false ) {
                Directory.CreateDirectory( oldDirectoryPath );
            }
            
            var oldFilePath = FilePathHelper.GetFilePath( oldDirectoryPath, fileInfo );
            
            if( File.Exists( oldFilePath ) ) {
                File.Delete( oldFilePath );
            }
            
            File.Move( filePath, oldFilePath );
        }
        
        private DownloadInfo DoDownloadFile( DownloadInfo info ) {
            try {
                var date = info.LastUpdate.ToString( "yyyy.MM.dd.HH.mm" );
                var url = info.VersionProcessing == 2
                    ? $"{info.Url}&last_import={date}"
                    : info.Url;
                    
                Measure(
                    () => {
                        var webClient = new WebClient();
                        webClient.DownloadFile( url, info.FilePath );
                    },
                    out var workTime );

                if( FileChecker.WithAnEnd( info.FilePath ) == false ) {
                    info.Error = DownloadError.Unfinished;
                    LogWriter.Log( $"{info.ShopName} ошибка { info.Error }", true );
                    return info;
                }
                
                info.Error = DownloadError.Ok;
                info.FileSize = new FileInfo( info.FilePath ).Length;
                info.DownloadTime = workTime;
                LogWriter.Log( $"Downloaded {info.ShopName}, time {workTime} " );
            }
            catch( Exception e ) {
                info.Error = GetError( e.Message );
                var errorMessage =
                    info.Error == DownloadError.UnknownError
                        ? e.Message
                        : info.Error.ToString();
                _context.AddMessage( $"{ info.ShopName }: { errorMessage }", true );
                LogWriter.Log( $"{info.ShopName} ошибка { errorMessage }", true );
            }

            if( info.HasError == false ) {
                _context.AddMessage( $"скачали { info.ShopName }" );
                _context.CalculatePercent();
                
                FileDownloaded?.Invoke( this, new DownloadEventArgs( info ) );
                
            }
            
            return info;
        }

        private static DownloadError GetError( string message ) =>
            message switch {
                "The remote server returned an error: (429) Unknown Status Code." => DownloadError.ManyRequests,
                "The remote server returned an error: (400) Bad Request." => DownloadError.ClosedStore,
                _ => DownloadError.UnknownError
            };

        private List<XmlFileInfo> GetFilesInfo() => _db.GetEnableShops();
    }
}