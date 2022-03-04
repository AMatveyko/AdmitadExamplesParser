// a.snegovoy@gmail.com

using System.Linq;

namespace Common.Entities
{
    public sealed class Category
    {
        public string Id { get; set; }
        public string[] Terms { get; set; }
        public string[] ExcludeTerms { get; set; }
        public string[] Fields { get; set; }
        public string[] ExcludeWordsFields { get; set; }
        public string[] SearchSpecify { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Name { get; set; }
        public string NameH1 { get; set; }
        public bool TakeUnisex { get; set; }
        public bool IsTermsEmpty() => Terms == null || Terms.Any() == false;
    }
}