// a.snegovoy@gmail.com

namespace Common.Settings
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
        public AdmitadApiSettings AdmitadSettings { get; set; }
        public string CtrCalculationType { get; set; }
        public bool UrlStatisticsDebuggingEnable { get; set; }
    }
}