using System.Collections.Generic;

namespace WebLogFilesTool.Workers
{
    internal interface IPathGetter
    {
        public List<string> Get();
    }
}