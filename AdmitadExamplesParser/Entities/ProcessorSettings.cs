// a.snegovoy@gmail.com

using Messenger;

namespace AdmitadExamplesParser.Entities
{
    internal sealed class ProcessorSettings
    {
        public int AttemptsToDownload { get; set; }
        public bool EnableExtendedStatistics { get; set; }
        public string DirectoryPath { get; set; }
        public string DuplicateFile { get; set; }
        public bool ShowStatistics { get; set; }
        public ElasticSearchClientSettings ElasticSearchClientSettings { get; set; }
        public MessengerSettings MessengerSettings { get; set; }
    }
}