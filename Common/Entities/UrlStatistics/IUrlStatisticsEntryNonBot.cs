// a.snegovoy@gmail.com

namespace Common.Entities
{
    public interface IUrlStatisticsEntryNonBot
    {
        int? NumberVisitsNonBot { get; set; }
        short? LastErrorCodeNonBot { get; set; }
    }
}