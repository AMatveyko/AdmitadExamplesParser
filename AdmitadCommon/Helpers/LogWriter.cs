// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;

namespace AdmitadCommon.Helpers
{
    public static class LogWriter {
        
        private const string Path = @"o:\admitad\logs\";
        private static readonly string _filePath;
        private static readonly Queue<string> _logs = new();

        static LogWriter()
        {
            _filePath = $"{Path}{DateTime.Now.ToString( "yyyy-MM-dd-HH-mm-ss" )}.log";
        }

        public static void Log( string line )
        {
            PrintToConsole( line );
            LogEnqueue( line );
        }

        private static void LogEnqueue( string line )
        {
            line = $"{DateTime.Now.ToString()} {line}";
            _logs.Enqueue( line );
        }
        
        public static void WriteLog()
        {
            foreach( var log in _logs ) {
                WriteToFile( log );
            }
        }
        
        private static void PrintLine( string line ) {
            WriteToFile( line );
        }

        private static void WriteToFile( string line )
        {
            File.AppendAllText( _filePath, $"{line}\n" );
        }
        
        private static void PrintToConsole( string line )
        {
            Console.WriteLine( line );
        }

    }
}