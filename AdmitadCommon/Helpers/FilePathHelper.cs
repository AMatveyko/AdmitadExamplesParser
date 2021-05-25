// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace AdmitadCommon.Helpers
{
    public static class FilePathHelper
    {
        public static string GetFilePath(
            string directoryPath,
            XmlFileInfo fileInfo ) =>
            $"{directoryPath}{fileInfo.NameLatin}.xml".Replace( "//", "/" );

        public static string CombinePath(
            string firstPath,
            string secondPath ) =>
            $"{firstPath.TrimEnd( '/' )}/{secondPath.TrimEnd( '/' )}".Replace( "//", "/" );
    }
}