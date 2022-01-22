// a.snegovoy@gmail.com

using System.Linq;

namespace Common.Entities
{
    public sealed class Tag
    {
        public string Id { get; set; }
        public string[] SearchTerms { get; set; }
        public string Gender { get; set; }
        public string[] Fields { get; set; }
        public int IdCategory { get; set; }
        public string[] SpecifyWords { get; set; }
        public string[] ExcludePhrase { get; set; }
        public string Title { get; set; }
        public string[] Categories { get; set; }
        public bool SearchAsPart { get; set; }
        public bool IsSearchTermsEmpty() => SearchTerms == null || SearchTerms.Any() == false;
        public int AddDate { get; set; }
    }
}