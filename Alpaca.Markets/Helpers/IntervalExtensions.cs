namespace Alpaca.Markets;

/// <summary>
/// Set of extensions methods for the <see cref="Interval{TItem}"/> structure.
/// </summary>
public static class IntervalExtensions
{
    /// <summary>
    /// Converts a <see cref="Interval{DateTime}"/> Interval into a <see cref="Interval{DateOnly}"/> Interval.
    /// </summary>
    /// <param name="interval">Input inclusive time interval for converting.</param>
    /// <returns>Date interval initialized with data from the original inclusive time interval.</returns>
    public static Interval<DateOnly> AsDateInterval(
        this Interval<DateTime> interval) =>
        new(interval.From.AsDateOnly(), interval.Into.AsDateOnly());

    /// <summary>
    /// Converts a <see cref="Interval{DateOnly}"/> Interval into a <see cref="Interval{DateTime}"/> Interval.
    /// </summary>
    /// <param name="interval">Input date interval for converting.</param>
    /// <returns>Inclusive time interval initialized with data from the original date interval.</returns>
    [UsedImplicitly]
    public static Interval<DateTime> AsTimeInterval(
        this Interval<DateOnly> interval) =>
        new(interval.From.AsDateTime(), interval.Into.AsDateTime());

    /// <summary>
    /// Gets exclusive open time interval starting from the <paramref name="from"/> date/time point.
    /// </summary>
    /// <param name="from">Starting point for filtering.</param>
    /// <returns>Exclusive open time interval.</returns>
    [UsedImplicitly]
    public static Interval<TItem> GetIntervalFromThat<TItem>(
        this TItem from)
        where TItem : struct, IComparable<TItem> =>
        new(from, null);

    /// <summary>
    /// Gets exclusive open time interval ending at the <paramref name="into"/> date/time point.
    /// </summary>
    /// <param name="into">Ending point for filtering.</param>
    /// <returns>Exclusive open time interval.</returns>
    [UsedImplicitly]
    public static Interval<TItem> GetIntervalTillThat<TItem>(
        this TItem into)
        where TItem : struct, IComparable<TItem> =>
        new(null, into);
}
