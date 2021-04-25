// a.snegovoy@gmail.com

using AdmitadCommon.Entities;

namespace TheStore.Api.Core.Sources.Entity
{
    public class RelinkTagContext : BackgroundBaseContext
    {
        public RelinkTagContext(
            string tagId )
            : base( $"RelinkTagContext:{tagId}" )
        {
            TagId = tagId;
        }
        public string TagId { get; }
        public string Title { get; set; }
    }
}