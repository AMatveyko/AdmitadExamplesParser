using System;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace WebLogFilesTool.Extensions
{
    internal static class MatchExtensions
    {
        public static IPAddress GetIPAddress(this Match m, string groupName) => IPAddress.Parse( GetValue(m, groupName));
        public static int GetIntValue(this Match m, string groupName) => int.Parse(GetNumericValueOrDefault(m, groupName));
        public static long GetLongValue(this Match m, string groupName) => long.Parse(GetNumericValueOrDefault(m, groupName));
        public static DateTime GetDateValue(this Match m, string groupName) =>
            DateTime.ParseExact(GetValue(m, groupName),"dd/MMM/yyyy:HH:mm:ss +0300",CultureInfo.InvariantCulture);
        public static string GetValue(this Match m, string groupName) => m.Groups[groupName].Value;

        private static string GetNumericValueOrDefault(Match m, string groupName) {
            var value = GetValue(m, groupName);
            return value == "" ? "0" : value;
        }
    }
}