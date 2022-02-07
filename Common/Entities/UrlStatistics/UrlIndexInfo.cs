namespace Common.Entities
{
    public sealed class UrlIndexInfo
    {
        public string Url { get; set; }
        public bool IsIndexed { get; set; }
        public string Error { get; set; }
    }
}