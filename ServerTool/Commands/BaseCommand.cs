// a.snegovoy@gmail.com

using System;
using System.Diagnostics;
using System.Threading;

using ServerTool.Entities;

namespace ServerTool.Commands
{
    internal abstract class BaseCommand : ICommand
    {

        private readonly Process _process;
        private readonly bool _needSleap;

        protected BaseCommand( string fileName, bool needSleep, string[] args )
        {
            _needSleap = needSleep;
            _process = new ();
            _process.StartInfo = new ProcessStartInfo {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = fileName,
                Arguments = JoinArgs( args ),
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
        }

        private static string JoinArgs( string[] args )
        {
            return string.Join( " ", args );
        }

        public CommandStatus Status { get; private set; } = CommandStatus.Ok;

        public string Execute()
        {
            try {
                return DoExecute();
            }
            catch( Exception e ) {
                Status = CommandStatus.Error;
                return e.Message;
            }
        }

        private string DoExecute()
        {
            _process.Start();
            var output = string.Concat( _process.StandardError.ReadToEnd(), _process.StandardOutput.ReadToEnd() );
            if( _needSleap ) {
                Thread.Sleep( 2000 );
            }
            return output;
        }
    }
}