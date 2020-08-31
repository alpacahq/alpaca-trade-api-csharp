using System;
#if !NETFRAMEWORK
using System.Runtime.InteropServices;
#endif

namespace Alpaca.Markets
{
    internal static class CustomTimeZone
    {
#if NETFRAMEWORK
        private static TimeZoneInfo Est { get; } =
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
#else
        private static TimeZoneInfo Est { get; } =
            TimeZoneInfo.FindSystemTimeZoneById(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "Eastern Standard Time"
                    : "America/New_York");
#endif

        public static DateTime ConvertFromEstToUtc(
            DateTime estDateTime) =>
#if NETSTANDARD1_3
            TimeZoneInfo.ConvertTime(estDateTime, Est, TimeZoneInfo.Utc);
#else
            TimeZoneInfo.ConvertTimeToUtc(estDateTime, Est);
#endif
    }
}
