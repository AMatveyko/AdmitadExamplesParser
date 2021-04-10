// a.snegovoy@gmail.com

using System.IO;
using System.Linq;

namespace AdmitadCommon.Helpers
{
    public static class FileSystemHelper {
        public static void PrepareDirectory( string directoryPath )
        {
            var files = new DirectoryInfo( directoryPath ).GetFiles();
            if( files.Any() == false ) {
                return;
            }

            foreach( var fileInfo in files ) {
                fileInfo.Delete();
            }
        }
    }
}