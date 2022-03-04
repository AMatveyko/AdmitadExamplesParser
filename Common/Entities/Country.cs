// a.snegovoy@gmail.com

using System.Linq;

namespace Common
{
    public sealed class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] SearchTerms { get; set; }
        
        public bool IsSearchTermsEmpty() => SearchTerms == null || SearchTerms.Any() == false;
    }
}