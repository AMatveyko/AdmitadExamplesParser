// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public interface IUrlStatisticsEntryIndexedYandex
    {
        bool? IndexedYandex { get; set; }
        DateTime? DateLastIndexCheckYandex { get; set; }
    }
}