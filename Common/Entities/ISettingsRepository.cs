// a.snegovoy@gmail.com

using System.Collections.Generic;

using Common.Settings;

namespace Common.Entities
{
    public interface ISettingsRepository
    {
        List<SettingsOption> GetSettingsOptions();
    }
}