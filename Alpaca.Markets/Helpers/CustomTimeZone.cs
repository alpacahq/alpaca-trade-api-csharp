using System;
using System.Runtime.InteropServices;

namespace Alpaca.Markets
{
    internal static class CustomTimeZone
    {
#if NETFRAMEWORK
        public static TimeZoneInfo Est { get; } =
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
#else
        public static TimeZoneInfo Est { get; } =
            TimeZoneInfo.FindSystemTimeZoneById(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "Eastern Standard Time"
                    : "America/New_York");
#endif
    }
}
