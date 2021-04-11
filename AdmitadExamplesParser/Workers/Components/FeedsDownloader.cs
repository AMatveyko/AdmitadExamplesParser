// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using AdmitadCommon.Entities;
using AdmitadCommon.Helpers;

using AdmitadExamplesParser.Entities;

using AdmitadSqlData.Helpers;

namespace AdmitadExamplesParser.Workers.Components
{
    internal sealed class FeedsDownloader : BaseComponent
    {

        private readonly int _numberAttempts;

        public FeedsDownloader(
            int numberAttempts )
            : base( ComponentType.Downloader )
        {
            _numberAttempts = Math.Max( numberAttempts, 1 );
        }
        
        public List<DownloadInfo> DownloadsAll(
            string filePath ) =>
            MeasureWorkTime( () => DoDownloadAll( filePath ) );

        private List<DownloadInfo> DoDownloadAll(
            string filePath )
        {
            var files = GetFilesInfo();
            var downloadInfos = files.AsParallel().Select( f => DownloadFile( f, filePath ) ).ToList();
            var withErrors = downloadInfos.Where( i => i.HasError ).ToList();
            if( withErrors.Any() ) {
                TryAgainIfNeed( withErrors );
            }
            
            var fileCount = Directory.GetFiles( filePath ).Length;
            LogWriter.Log( $"Files downloaded {fileCount} from {files.Count}", true );
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
                LogWriter.Log( $"Attempt {i}/{_numberAttempts} to download feeds.", true );
                var downloaded = needDownload.AsParallel().Select( DoDownloadFile ).ToList();
                needDownload = GetNeedDownload( downloaded );
                if( needDownload.Any() == false ) {
                    LogWriter.Log( "All have finished.", true );
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
            string directoryPath )
        {
            var downloadInfo = new DownloadInfo {
                ShopName = fileInfo.Name,
                Url = fileInfo.XmlFeed,
                FilePath = $"{directoryPath}{fileInfo.NameLatin}.xml".Replace( "//", "/" )
            };
            return DoDownloadFile( downloadInfo );
        }
        
        private DownloadInfo DoDownloadFile( DownloadInfo info ) {
            try {
                Measure(
                    () => {
                        var webClient = new WebClient();
                        webClient.DownloadFile( info.Url, info.FilePath );
                    },
                    out var workTime );
                info.Error = DownloadError.Ok;
                info.DownloadTime = workTime;
                LogWriter.Log( $"Downloaded {info.ShopName}, time {workTime} " );
            }
            catch( Exception e ) {
                info.Error = GetError( e.Message );
                var errorMessage =
                    info.Error == DownloadError.UnknownError
                        ? e.Message
                        : info.Error.ToString();
                LogWriter.Log( $"Error {info.ShopName}: { errorMessage }", true );
            }

            return info;
        }

        private static DownloadError GetError( string message ) =>
            message switch {
                "The remote server returned an error: (429) Unknown Status Code." => DownloadError.ManyRequests,
                "The remote server returned an error: (400) Bad Request." => DownloadError.ClosedStore,
                _ => DownloadError.UnknownError
            };

        private static List<XmlFileInfo> GetFilesInfo() => DbHelper.GetEnableShops();
    }
}