namespace Alpaca.Markets;

/// <summary>
/// Set of extensions methods for the <see cref="IIntervalCalendar"/> interface.
/// </summary>
public static class IntervalCalenderExtensions
{
    /// <summary>
    /// Gets trading date (midnight or 00:00 time point).
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateOnly GetTradingDate(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).GetTradingDateFast();

    /// <summary>
    /// Gets trading open time in EST time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetTradingOpenTimeEst(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).GetTradingOpenTimeEstFast();

    /// <summary>
    /// Gets trading close time in EST time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetTradingCloseTimeEst(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).GetTradingCloseTimeEstFast();

    /// <summary>
    /// Gets trading open time in UTC time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetTradingOpenTimeUtc(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).GetTradingOpenTimeUtcFast();

    /// <summary>
    /// Gets trading close time in UTC time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetTradingCloseTimeUtc(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).GetTradingCloseTimeUtcFast();

    /// <summary>
    /// Gets session open time in EST time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetSessionOpenTimeEst(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).Session.OpenEst.DateTime;

    /// <summary>
    /// Gets session close time in EST time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetSessionCloseTimeEst(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).Session.CloseEst.DateTime;

    /// <summary>
    /// Gets session open time in UTC time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetSessionOpenTimeUtc(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).Session.OpenEst.UtcDateTime;

    /// <summary>
    /// Gets session close time in UTC time zone.
    /// </summary>
    /// <param name="calendar">Alpaca calendar data.</param>
    [UsedImplicitly]
    public static DateTime GetSessionCloseTimeUtc(
        this IIntervalCalendar calendar) =>
        calendar.EnsureNotNull(nameof(calendar)).Session.CloseEst.UtcDateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static DateOnly GetTradingDateFast(
        this IIntervalCalendar calendar) =>
        DateOnly.FromDateTime(calendar.Session.OpenEst.DateTime);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static DateTime GetTradingOpenTimeEstFast(
        this IIntervalCalendar calendar) =>
        calendar.Trading.OpenEst.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static DateTime GetTradingCloseTimeEstFast(
        this IIntervalCalendar calendar) =>
        calendar.Trading.CloseEst.DateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static DateTime GetTradingOpenTimeUtcFast(
        this IIntervalCalendar calendar) =>
        calendar.Trading.OpenEst.UtcDateTime;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static DateTime GetTradingCloseTimeUtcFast(
        this IIntervalCalendar calendar) =>
        calendar.Trading.CloseEst.UtcDateTime;
}
