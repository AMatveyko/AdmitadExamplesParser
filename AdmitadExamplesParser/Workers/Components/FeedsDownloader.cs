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

        public List<DownloadInfo> DownloadsAll( string filePath ) =>
            MeasureWorkTime( () => DoDownloadAll( filePath ) );

        private List<DownloadInfo> DoDownloadAll( string filePath )
        {
            var files = GetFilesInfo();
            var downloadInfos = files.Take( 20 ).AsParallel().Select( f => DownloadFile( f, filePath ) ).ToList();
            var fileCount = Directory.GetFiles( filePath ).Length;
            LogWriter.Log( $"Files downloaded {fileCount} from {files.Count}" );
            return downloadInfos;
        }

        private DownloadInfo DownloadFile( XmlFileInfo fileInfo, string directoryPath )
        {
            var downloadInfo = new DownloadInfo {
                ShopName = fileInfo.Name,
                Url = fileInfo.XmlFeed,
                FilePath = $"{directoryPath}{fileInfo.NameLatin}.xml".Replace( "//", "/" )
            };
            try {
                Measure(
                    () => {
                        var webClient = new WebClient();
                        webClient.DownloadFile( fileInfo.XmlFeed, downloadInfo.FilePath );
                    }, out var workTime );
                downloadInfo.DownloadTime = workTime;
                LogWriter.Log( $"Downloaded {fileInfo.Name}, time { workTime } ");
            }
            catch( Exception e ) {
                downloadInfo.Error = e.Message;
                LogWriter.Log( $"Error { fileInfo.Name }: { e.Message }" );
            }

            return downloadInfo;
        }

        private static List<XmlFileInfo> GetFilesInfo() => DbHelper.GetEnableShops();

        public FeedsDownloader()
            : base( ComponentType.Downloader ) { }
    }
}