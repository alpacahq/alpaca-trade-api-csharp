using System;

namespace Alpaca.Markets
{
    internal static class DateTimeHelper
    {
        private const Int64 NanosecondsInMilliseconds = 1_000_000;

        private static readonly Int64 _timeSpanMaxValueMilliseconds =
            (Int64)TimeSpan.MaxValue.TotalMilliseconds;

        public static String DateFormat => "yyyy-MM-dd";

        public static DateTime AsUtcDateTime(
            this in DateTime value) =>
            DateTime.SpecifyKind(value, DateTimeKind.Utc);

        public static DateTime FromUnixTimeSeconds(
            this in Int64 linuxTimeStamp) =>
            DateTimeOffset.FromUnixTimeSeconds(linuxTimeStamp).UtcDateTime;

        public static DateTime FromUnixTimeMilliseconds(
            this in Int64 linuxTimeStamp) =>
            DateTimeOffset.FromUnixTimeMilliseconds(linuxTimeStamp).UtcDateTime;

        public static DateTime FromUnixTimeNanoseconds(
            this in Int64 linuxTimeStamp) =>
            linuxTimeStamp > _timeSpanMaxValueMilliseconds
                ? FromUnixTimeMilliseconds(linuxTimeStamp / NanosecondsInMilliseconds)
                : FromUnixTimeMilliseconds(linuxTimeStamp);

        public static Int64 IntoUnixTimeSeconds(
            this in DateTime dateTime) =>
            new DateTimeOffset(dateTime).ToUnixTimeSeconds();

        public static Int64 IntoUnixTimeMilliseconds(
            this in DateTime dateTime) =>
            new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();

        public static Int64 IntoUnixTimeNanoseconds(
            this in DateTime dateTime) =>
            new DateTimeOffset(dateTime).ToUnixTimeMilliseconds() * NanosecondsInMilliseconds;
    }
}
