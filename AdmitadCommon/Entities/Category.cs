// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public class Category
    {
        public string Id { get; set; }
        public string[] Terms { get; set; }
        public string[] ExcludeTerms { get; set; }
        public string[] Fields { get; set; }
        public string[] ExcludeWordsFields { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
    }
}