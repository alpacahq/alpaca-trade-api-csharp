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
        // TODO: olegra - we can use the method above for the .NET 6.0 and later
        private static TimeZoneInfo Est { get; } =
            TimeZoneInfo.FindSystemTimeZoneById(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "Eastern Standard Time"
                    : "America/New_York");
#endif

        public static DateTime ConvertFromEstToUtc(
            DateTime estDateTime) =>
            TimeZoneInfo.ConvertTimeToUtc(estDateTime, Est);
    }
}
