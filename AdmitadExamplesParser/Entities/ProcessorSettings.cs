// a.snegovoy@gmail.com

using System.Collections.Generic;

namespace AdmitadExamplesParser.Entities
{
    internal sealed class ProcessorSettings
    {
        public bool EnableExtendedStatistics { get; set; }
        public string DirectoryPath { get; set; }
        public string DuplicateFile { get; set; }
        public bool ShowStatistics { get; set; }
        public ElasticSearchClientSettings ElasticSearchClientSettings { get; set; }
    }
}