// a.snegovoy@gmail.com

using ServerTool.Commands;

namespace ServerTool.Entities
{
    internal sealed class Service : IService
    {
        public Service( string name, RestartService restart, StatusService status, bool needRestart ) =>
            ( Name, Status, Restart, NeedRestart ) = ( name, status, restart, needRestart );
        
        public bool NeedRestart { get; }
        public string Name { get; }
        public ICommand Status { get; }
        public ICommand Restart { get; }
    }
}