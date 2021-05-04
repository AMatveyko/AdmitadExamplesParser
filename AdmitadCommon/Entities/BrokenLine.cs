// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
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