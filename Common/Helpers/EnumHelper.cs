using System;

namespace Common.Helpers
{
    public static class EnumHelper<T> where T : Enum
    {
        public static T GetValueByName(string name) => (T)Enum.Parse(typeof(T), name);
    }
}
