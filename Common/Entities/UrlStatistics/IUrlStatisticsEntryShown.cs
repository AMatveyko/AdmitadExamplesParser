// a.snegovoy@gmail.com

using System;

namespace Common.Entities
{
    public interface IUrlStatisticsEntryShown
    {
        public DateTime? DateLastShowYandex { get; set; }
        public DateTime? DateLastShowGoogle { get; set; }
    }
}