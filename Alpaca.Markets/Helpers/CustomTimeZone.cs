#if !(NETFRAMEWORK || NET6_0_OR_GREATER)
using System.Runtime.InteropServices;
#endif

namespace Alpaca.Markets;

internal static class CustomTimeZone
{
#if NETFRAMEWORK || NET6_0_OR_GREATER
    private static TimeZoneInfo Est { get; } =
        TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
#else
    private static TimeZoneInfo Est { get; } =
        TimeZoneInfo.FindSystemTimeZoneById(
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? "Eastern Standard Time"
                : "America/New_York");
#endif

    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ConvertFromUtcToEst(
        DateTime utcDateTime) =>
        TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, Est);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime ConvertFromEstToUtc(
        DateTime estDateTime) =>
        TimeZoneInfo.ConvertTimeToUtc(estDateTime, Est);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset AsDateTimeOffset(
        in DateOnly date, in TimeOnly time) =>
        date.ToDateTime(time, DateTimeKind.Unspecified).asDateTimeOffset();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DateTimeOffset asDateTimeOffset(
        this DateTime estDateTime) =>
        new (estDateTime, Est.GetUtcOffset(estDateTime));
}
