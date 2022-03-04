// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public interface IUrlStatisticsEntryYandex
    {
        DateTime? LastVisitDateYandex { get; set; }
        int? NumberVisitsYandex { get; set; }
        short? LastErrorCodeYandex { get; set; }
        
    }
}