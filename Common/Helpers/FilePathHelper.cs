// a.snegovoy@gmail.com

using System.IO;

using Common.Entities;

namespace Common.Helpers
{
    public static class FilePathHelper
    {
        public static string GetFilePath( string directoryPath, string feedId, ShopInfo fileInfo ) =>
            fileInfo.VersionProcessing == 2
                ? $"{directoryPath}{fileInfo.NameLatin}Changes_{feedId}.xml".Replace( "//", "/" )
                : $"{directoryPath}{fileInfo.NameLatin}_{feedId}.xml".Replace( "//", "/" );

        public static string CombinePath(
            string firstPath,
            string secondPath ) =>
            $"{firstPath}/{secondPath}".Replace( "//", "/");
            //Path.Combine( firstPath, secondPath );
    }
}