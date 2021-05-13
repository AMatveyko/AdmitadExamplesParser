// a.snegovoy@gmail.com

using ServerTool.Entities;

namespace ServerTool.Commands
{
    internal sealed class RestartService : BaseCommand
    {
        public RestartService( string serviceName )
            : base(
                Systemctl.Path,
                true,
                new [] { ServiseAction.Restart.ToString().ToLower(), serviceName } ) { }
    }
}