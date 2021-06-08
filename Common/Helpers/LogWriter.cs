// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Common.Entities;

namespace Common.Helpers
{
    public static class LogWriter {
        
        private const string Path = @"o:\admitad\logs\";
        private static readonly string _filePath;
        private static readonly Queue<Message> _messages = new Queue<Message>();

        static LogWriter()
        {
            _filePath = $"{Path}{DateTime.Now.ToString( "yyyy-MM-dd-HH-mm-ss" )}.log";
        }

        public static void Log( string line, bool important = false )
        {
            var message = CreateMessage( line, important );
            ProcessMessage( message );
        }

        private static Message CreateMessage( string line, bool important ) =>
            new Message(important, DateTime.Now, line);
        
        private static void ProcessMessage( Message message )
        {
            PrintToConsole( message );
            LogEnqueue( message );
        }

        private static void LogEnqueue( Message message )
        {
            _messages.Enqueue( message );
        }
        
        public static void WriteLog( Action<string> sendLog )
        {
            WriteLogToFile();
            SendLogRemote( sendLog );
        }

        private static void SendLogRemote( Action<string> sendLog ) {
            sendLog?.Invoke( GetLogForRemote() );
        }

        private static string GetLogForRemote()
        {
            var fullMessage = new StringBuilder();
            foreach( var message in _messages.Where( m => m.Important ) ) {
                fullMessage.AppendLine( message.Text );
            }
            return fullMessage.ToString();
        }
        
        private static void WriteLogToFile()
        {
            foreach( var message in _messages ) {
                File.AppendAllText( _filePath, $"{message}\n" );
            }
        }
        
        private static void PrintToConsole( Message message )
        {
            Console.WriteLine( message.Text );
        }

    }
}