// a.snegovoy@gmail.com

using System;

using AdmitadCommon.Entities;

using Messenger;

using NLog;

namespace ServerTool
{
    internal sealed class Loggers
    {
        private static readonly Logger Logger = LogManager.GetLogger( "Logger" );
        private readonly Messenger.Messenger _messenger;

        public Loggers( MessengerSettings settings )
        {
            _messenger = new Messenger.Messenger( settings );
        }
        
        public void Error( Exception e ) {
            Console.WriteLine( e.Message );
            Logger.Error( e );
            _messenger.Send( e.Message );
        }

        public void Info( string message, bool needSend = false )
        {
            Console.WriteLine( message );
            Logger.Info( message );
            if( needSend ) {
                _messenger.Send( message );
            }
        }
    }
}