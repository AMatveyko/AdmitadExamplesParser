// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Web.Common.Entities;

namespace Web.Common.Helpers
{
    public static class ProxyDistributor
    {

        public static IEnumerable<ProxyInfo> GetProxies( IEnumerable<ProxyInfo> infos ) =>
            DoGet( infos.ToList() );

        private static List<ProxyInfo> DoGet( IList<ProxyInfo> workProxies )
        {
            workProxies.Add( null ); //добавляем вместо себя

            var forUse = new List<ProxyInfo>();
            var rnd = new Random();
            
            while( forUse.Count <= 3 && workProxies.Count > 0 ) {
                var set = rnd.Next( 0, workProxies.Count );
                forUse.Add( workProxies[set] );
                workProxies.RemoveAt( set );
            }
            
            return forUse;
        }
    }
}