namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="ICalendar"/> interface.
/// </summary>
public static class CalendarExtensions
{
    /// <summary>
    /// Converts the <see cref="ICalendar"/> open/close times into the
    /// <see cref="IInclusiveTimeInterval"/> instance for using in requests.
    /// </summary>
    /// <param name="calendar">The source open/close times information.</param>
    /// <returns>The inclusive time interval constructed from the <paramref name="calendar"/> data.</returns>
    [UsedImplicitly]
    public static IInclusiveTimeInterval AsInclusiveTimeIntervalUtc(
        this ICalendar calendar) =>
        TimeInterval.GetInclusive(
            calendar.EnsureNotNull(nameof(calendar)).TradingOpenTimeUtc,
            calendar.TradingCloseTimeUtc);

    /// <summary>
    /// Converts the <see cref="ICalendar"/> open/close times into the
    /// <see cref="IExclusiveTimeInterval"/> instance for using in requests.
    /// </summary>
    /// <param name="calendar">The source open/close times information.</param>
    /// <returns>The exclusive time interval constructed from the <paramref name="calendar"/> data.</returns>
    [UsedImplicitly]
    public static IExclusiveTimeInterval AsExclusiveTimeIntervalUtc(
        this ICalendar calendar) =>
        TimeInterval.GetExclusive(
            calendar.EnsureNotNull(nameof(calendar)).TradingOpenTimeUtc,
            calendar.TradingCloseTimeUtc);
}
