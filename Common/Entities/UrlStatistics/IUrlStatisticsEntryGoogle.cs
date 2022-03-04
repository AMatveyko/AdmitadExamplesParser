// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public interface IUrlStatisticsEntryGoogle
    {
        DateTime? LastVisitDateGoogle { get; set; }
        int? NumberVisitsGoogle { get; set; }
        short? LastErrorCodeGoogle { get; set; }
    }
}