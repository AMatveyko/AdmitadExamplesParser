// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace TheStore.Api.Core.Sources.Entity
{
    public class RelinkCategoryContext : BackgroundBaseContext
    {
        public string CategoryId { get; set; }
    }
}