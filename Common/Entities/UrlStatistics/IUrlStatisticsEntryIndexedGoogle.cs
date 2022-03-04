// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public interface IUrlStatisticsEntryIndexedGoogle
    {
        bool? IndexedGoogle { get; set; }
        DateTime? DateLastIndexCheckGoogle { get; set; }
    }
}