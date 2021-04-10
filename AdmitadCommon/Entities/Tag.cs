// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public sealed class Tag
    {
        public string Id { get; set; }
        public string[] SearchTerms { get; set; }
        public string Gender { get; set; }
        public string[] Fields { get; set; }
        public int IdCategory { get; set; }
    }
}