// a.snegovoy@gmail.com

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Admitad.Converters.Helpers
{
    public class FileChecker
    {

        private static readonly Regex EndFile = new Regex( @"<\/offers>", RegexOptions.Compiled );
        
        public static bool WithAnEnd( string path )
        {
            const int endLength = 350;
            using var fileStream = new FileStream( path, FileMode.Open );
            fileStream.Seek( -endLength, SeekOrigin.End );
            var endFile = new byte[endLength];
            fileStream.Read( endFile, 0, endFile.Length );
            var text = Encoding.Default.GetString( endFile );
            return EndFile.IsMatch( text );
        }
    }
}