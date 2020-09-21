using System;
using System.Globalization;

namespace Alpaca.Markets
{
    internal static class DateTimeHelper
    {
#if NET45
        private static readonly DateTime _epoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
#endif

        private const Int64 NanosecondsInMilliseconds = 1_000_000;

        private static readonly Int64 _timeSpanMaxValueMilliseconds =
            (Int64)TimeSpan.MaxValue.TotalMilliseconds;

        public static String DateFormat { get; } = "yyyy-MM-dd";

        public static String AsDateString(in this DateTime dateTime) =>
            dateTime.ToString(DateFormat, CultureInfo.InvariantCulture);

        public static DateTime AsUtcDateTime(
            this in DateTime value) =>
            DateTime.SpecifyKind(value, DateTimeKind.Utc);

        public static DateTime FromUnixTimeSeconds(
            this in Int64 linuxTimeStamp) =>
#if NET45
            _epoch.Add(TimeSpan.FromSeconds(linuxTimeStamp));
#else
            DateTimeOffset.FromUnixTimeSeconds(linuxTimeStamp).UtcDateTime;
#endif

        public static DateTime FromUnixTimeMilliseconds(
            this in Int64 linuxTimeStamp) =>
#if NET45
            _epoch.Add(TimeSpan.FromMilliseconds(linuxTimeStamp));
#else
            DateTimeOffset.FromUnixTimeMilliseconds(linuxTimeStamp).UtcDateTime;
#endif

        public static DateTime FromUnixTimeNanoseconds(
            this in Int64 linuxTimeStamp) =>
            linuxTimeStamp > _timeSpanMaxValueMilliseconds
                ? FromUnixTimeMilliseconds(linuxTimeStamp / NanosecondsInMilliseconds)
                : FromUnixTimeMilliseconds(linuxTimeStamp);

        public static Int64 IntoUnixTimeSeconds(
            this in DateTime dateTime) =>
#if NET45
            (Int64)(dateTime.Subtract(_epoch)).TotalSeconds;
#else
            new DateTimeOffset(dateTime).ToUnixTimeSeconds();
#endif

        public static Int64 IntoUnixTimeMilliseconds(
            this in DateTime dateTime) =>
#if NET45
            (Int64)(dateTime.Subtract(_epoch)).TotalMilliseconds;
#else
            new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
#endif

        public static Int64 IntoUnixTimeNanoseconds(
            this in DateTime dateTime) =>
#if NET45
            (Int64)(dateTime.Subtract(_epoch).TotalMilliseconds * NanosecondsInMilliseconds);
#else
            new DateTimeOffset(dateTime).ToUnixTimeMilliseconds() * NanosecondsInMilliseconds;
#endif
    }
}
