namespace Alpaca.Markets;

internal static class CustomTimeZone
{
    private const String WindowsTimeZoneId = "Eastern Standard Time";

    private const String IanaTimeZoneId = "America/New_York";

    private static TimeZoneInfo Est { get; } =
        TimeZoneInfo.FindSystemTimeZoneById(
            shouldUseWindowsTimeZoneId() ? WindowsTimeZoneId : IanaTimeZoneId);

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
        new(estDateTime, Est.GetUtcOffset(estDateTime));

    private static Boolean shouldUseWindowsTimeZoneId() =>
#if NETFRAMEWORK
        true;  // The .NET Framework 4.x exists only on Windows platform
#elif NET6_0_OR_GREATER
        false; // The .NET 6 can use both IANA and Windows time zone names
#else
        System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
            System.Runtime.InteropServices.OSPlatform.Windows);
#endif
}
