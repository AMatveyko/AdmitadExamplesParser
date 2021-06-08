// a.snegovoy@gmail.com

using ServerTool.Commands;
using ServerTool.Entities;

namespace ServerTool.Workers
{
    internal static class ServiceBuilder
    {
        public static IService Create( (string,bool) serviceSettings )
        {
            var status = new StatusService( serviceSettings.Item1 );
            var restart = new RestartService( serviceSettings.Item1 );
            return new Service( serviceSettings.Item1, restart, status, serviceSettings.Item2 );
        }
    }
}