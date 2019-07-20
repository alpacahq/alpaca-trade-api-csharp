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
            DateTime utcDate,
            DateTime estTime) =>
#if NETSTANDARD1_6
            TimeZoneInfo.ConvertTime(
                utcDate.Date.Add(estTime.TimeOfDay),
                Est, TimeZoneInfo.Utc);
#else
            TimeZoneInfo.ConvertTimeToUtc(
                utcDate.Date.Add(estTime.TimeOfDay),
                Est);
#endif
    }
}
