// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace TheStore.Api.Core.Sources.Entity
{
    public class RelinkTagContext : BackgroundBaseContext
    {
        public string TagId { get; set; }
    }
}