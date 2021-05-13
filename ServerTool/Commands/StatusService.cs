// a.snegovoy@gmail.com

using ServerTool.Entities;

namespace ServerTool.Commands
{
    internal sealed class StatusService : BaseCommand
    {
        public StatusService( string serviceName )
            : base(
                Systemctl.Path, 
                false, 
                new [] { ServiseAction.Status.ToString().ToLower(), serviceName } ) { }
    }
}