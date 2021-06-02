// a.snegovoy@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;

using Web.Common.Entities;

namespace Web.Common.Helpers
{
    public static class ProxyDistributor
    {
        
        public static List<ProxyInfo> GetProxies( List<ProxyInfo> infos )
        {
            infos.Add( null ); //добавляем вместо себя
            var rnd = new Random();
            var set = rnd.Next( 1, 4 );
            var forUse = set switch {
                1 => new[] {3, 1, 2},
                2 => new[] {3, 2, 1},
                _ => new[] {2, 1, 3}
            };
            return forUse.Select( i => infos[i] ).ToList();
        }
    }
}