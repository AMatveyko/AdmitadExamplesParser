// a.snegovoy@gmail.com

using Common.Entities;

namespace Common.Helpers
{
    public static class FilePathHelper
    {
        public static string GetFilePath( string directoryPath, XmlFileInfo fileInfo ) =>
            fileInfo.VersionProcessing == 2
                ? $"{directoryPath}{fileInfo.NameLatin}Changes.xml".Replace( "//", "/" )
                : $"{directoryPath}{fileInfo.NameLatin}.xml".Replace( "//", "/" );

        public static string CombinePath( string firstPath, string secondPath ) =>
            $"{firstPath}/{secondPath}".Replace( "//", "/");
    }
}