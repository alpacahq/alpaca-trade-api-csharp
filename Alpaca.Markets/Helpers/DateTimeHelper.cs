using System;

namespace Alpaca.Markets
{
    internal static class DateTimeHelper
    {
#if NET45
        private static readonly DateTime _epoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#endif

        private const Int64 NANOSECONDS_IN_MILLISECONDS = 1_000_000;

        private static readonly Int64 _timeSpanMaxValueMilliseconds =
            (Int64)TimeSpan.MaxValue.TotalMilliseconds;

        public static DateTime FromUnixTimeNanoseconds(
            Int64 linuxTimeStamp) =>
            linuxTimeStamp > _timeSpanMaxValueMilliseconds
                ? FromUnixTimeMilliseconds(linuxTimeStamp / NANOSECONDS_IN_MILLISECONDS)
                : FromUnixTimeMilliseconds(linuxTimeStamp);

        public static DateTime FromUnixTimeMilliseconds(
            Int64 linuxTimeStamp) =>
#if NET45
            _epoch.Add(TimeSpan.FromMilliseconds(linuxTimeStamp));
#else
            DateTime.SpecifyKind(
                DateTimeOffset.FromUnixTimeMilliseconds(linuxTimeStamp)
                    .DateTime, DateTimeKind.Utc);
#endif

        public static DateTime FromUnixTimeSeconds(
            Int64 linuxTimeStamp) =>
#if NET45
            _epoch.Add(TimeSpan.FromSeconds(linuxTimeStamp));
#else
            DateTime.SpecifyKind(
                DateTimeOffset.FromUnixTimeSeconds(linuxTimeStamp)
                    .DateTime, DateTimeKind.Utc);
#endif

        public static Int64 GetUnixTimeMilliseconds(
            DateTime dateTime) =>
#if NET45
            (Int64)(dateTime.Subtract(_epoch)).TotalMilliseconds;
#else
            new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
#endif
    }
}
