// a.snegovoy@gmail.com

namespace AdmitadCommon.Entities
{
    public sealed class ProcessorSettings
    {
        public int AttemptsToDownload { get; set; }
        public bool EnableExtendedStatistics { get; set; }
        public string DirectoryPath { get; set; }
        public string DuplicateFile { get; set; }
        public bool ShowStatistics { get; set; }
        public ElasticSearchClientSettings ElasticSearchClientSettings { get; set; }
        public TelegramSettings TelegramSettings { get; set; }
    }
}