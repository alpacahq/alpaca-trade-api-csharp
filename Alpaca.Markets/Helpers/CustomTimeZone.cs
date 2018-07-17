using System;

namespace Alpaca.Markets
{
    internal static class CustomTimeZone
    {
        public static TimeZoneInfo Est { get; } =
            TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
    }
}
