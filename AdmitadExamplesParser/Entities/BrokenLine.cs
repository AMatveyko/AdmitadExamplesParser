// a.snegovoy@gmail.com

namespace AdmitadExamplesParser.Entities
{
    public sealed class BrokenLine
    {
        public BrokenLine(
            string error,
            string line )
        {
            Error = error;
            Line = line;
        }
        public string Error { get; }
        public string Line { get; }
    }
}