// a.snegovoy@gmail.com

using System;

using Common.Helpers;

namespace Common.Entities
{
    public sealed class UrlStatisticEntry : IUrlStatisticsEntryYandex, IUrlStatisticsEntryGoogle, IUrlStatisticsEntryNonBot, IUrlStatisticsEntryIndexedGoogle, IUrlStatisticsEntryIndexedYandex, IUrlStatisticsEntryShown
    {

        public UrlStatisticEntry( string url ) {
            Url = url;
            Id = HashHelper.GetMd5Hash( url );
        }
        
        public string Id { get; }
        public string Url { get; }
        
        public DateTime? LastVisitDateYandex { get; set; }
        public int? NumberVisitsYandex { get; set; }
        public short? LastErrorCodeYandex { get; set; }
        public bool? IndexedYandex { get; set; }
        public DateTime? DateLastIndexCheckYandex { get; set; }
        public DateTime? DateLastShowYandex { get; set; }

        public DateTime? LastVisitDateGoogle { get; set; }
        public int? NumberVisitsGoogle { get; set; }
        public short? LastErrorCodeGoogle { get; set; }
        public bool? IndexedGoogle { get; set; }
        public DateTime? DateLastIndexCheckGoogle { get; set; }
        public DateTime? DateLastShowGoogle { get; set; }
        public int? NumberVisitsNonBot { get; set; }
        public short? LastErrorCodeNonBot { get; set; }
    }
}