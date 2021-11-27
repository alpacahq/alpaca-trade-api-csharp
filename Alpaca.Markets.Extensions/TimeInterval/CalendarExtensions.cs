namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="ICalendar"/> interface.
/// </summary>
public static class CalendarExtensions
{
    /// <summary>
    /// Converts the <see cref="ICalendar"/> open/close times into the
    /// <see cref="Interval{DateTime}"/> instance for using in requests.
    /// </summary>
    /// <param name="calendar">The source open/close times information.</param>
    /// <returns>The inclusive time interval constructed from the <paramref name="calendar"/> data.</returns>
    [UsedImplicitly]
    public static Interval<DateTime> AsIntervalUtc(
        this ICalendar calendar) =>
        new (calendar.EnsureNotNull(nameof(calendar)).TradingOpenTimeUtc, calendar.TradingCloseTimeUtc);

    /// <summary>
    /// Converts the <see cref="ICalendar"/> open/close times into the
    /// <see cref="IInclusiveTimeInterval"/> instance for using in requests.
    /// </summary>
    /// <param name="calendar">The source open/close times information.</param>
    /// <returns>The inclusive time interval constructed from the <paramref name="calendar"/> data.</returns>
    [UsedImplicitly]
    [Obsolete("Use AsIntervalUtc() extension method instead of this one.", false)]
    public static IInclusiveTimeInterval AsInclusiveTimeIntervalUtc(
        this ICalendar calendar) => TimeInterval
        .GetInclusiveIntervalFromThat(calendar.EnsureNotNull(nameof(calendar)).TradingOpenTimeUtc)
        .WithInto(calendar.TradingCloseTimeUtc);

    /// <summary>
    /// Converts the <see cref="ICalendar"/> open/close times into the
    /// <see cref="IExclusiveTimeInterval"/> instance for using in requests.
    /// </summary>
    /// <param name="calendar">The source open/close times information.</param>
    /// <returns>The exclusive time interval constructed from the <paramref name="calendar"/> data.</returns>
    [UsedImplicitly]
    [Obsolete("Use AsIntervalUtc() extension method instead of this one.", false)]
    public static IExclusiveTimeInterval AsExclusiveTimeIntervalUtc(
        this ICalendar calendar) => TimeInterval
        .GetExclusiveIntervalFromThat(calendar.EnsureNotNull(nameof(calendar)).TradingOpenTimeUtc)
        .WithInto(calendar.TradingCloseTimeUtc);
}
