// a.snegovoy@gmail.com

using System.Collections.Generic;
using System.Linq;

using TheStore.Api.Front.Data.Repositories;

using Web.Common.Entities;

namespace TheStore.Api.Front.Entity
{
    public sealed class Proxies
    {

        public Proxies( TheStoreRepository repository )
        {
            Infos = repository.GetProxies()
                .Select( p => new ProxyInfo( p.Host, p.Port, p.User, p.Password ) )
                .ToList();
        }
        
        public List<ProxyInfo> Infos { get; }
    }
}