namespace Alpaca.Markets;

/// <summary>
/// Encapsulates interval starting and ending points - used for date/time filtering in requests.
/// </summary>
/// <typeparam name="TItem">Interval range data type.</typeparam>
public readonly record struct Interval<TItem>
    where TItem : struct, IComparable<TItem>
{
    /// <summary>
    /// Creates the new instance of the <see cref="Interval{TItem}"/> structure.
    /// </summary>
    public Interval()
        : this (null)
    {
    }

    /// <summary>
    /// Creates the new instance of the <see cref="Interval{TItem}"/> structure.
    /// </summary>
    /// <param name="from">Initial value for the <see cref="From"/> property.</param>
    /// <param name="into">Initial value for the <see cref="Into"/> property.</param>
    public Interval(
        TItem? from,
        TItem? into)
    {
        if (from is not null &&
            into is not null &&
            from.Value.CompareTo(into.Value) > 0)
        {
            throw new ArgumentException("Time interval should be valid.");
        }

        From = from;
        Into = into;
    }

    internal Interval(
        TItem? value)
    {
        From = value;
        Into = value;
    }

    /// <summary>
    /// Gets the starting point of interval.
    /// /// </summary>
    public TItem? From { get; }

    /// <summary>
    /// Gets the ending point of interval.
    /// </summary>
    public TItem? Into { get; }

    /// <summary>
    /// Deconstructs the <see cref="Interval{TItem}"/> instance
    /// into two <see cref="Nullable{TItem}"/> values (tuple).
    /// </summary>
    /// <param name="from">Interval starting point.</param>
    /// <param name="into">Interval ending point.</param>
    [UsedImplicitly]
    public void Deconstruct(
        out TItem? from,
        out TItem? into)
    {
        from = From;
        into = Into;
    }

    /// <summary>
    /// Gets boolean flag that signals the time interval is empty (both start and end date equal to <c>null</c>).
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="From"/> and <see cref="Into"/> are equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    public Boolean IsEmpty() => Into is null && From is null;

    /// <summary>
    /// Gets boolean flag that signals the time interval is open (start or end date equal to <c>null</c>).
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="From"/> or <see cref="Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    public Boolean IsOpen() => Into is null ^ From is null;

    /// <summary>
    /// Creates new instance of <see cref="Interval{TItem}"/> object
    /// with the modified <see cref="Into"/> property value.
    /// </summary>
    /// <param name="into">New ending point for interval.</param>
    /// <returns>The new instance of <see cref="Interval{TItem}"/> object.</returns>
    [UsedImplicitly]
    public Interval<TItem> WithInto(TItem into) => new(From, into);

    /// <summary>
    /// Creates new instance of <see cref="Interval{TItem}"/> object
    /// with the modified <see cref="From"/> property value.
    /// </summary>
    /// <param name="from">New starting point for interval.</param>
    /// <returns>The new instance of <see cref="Interval{TItem}"/> object.</returns>
    [UsedImplicitly]
    public Interval<TItem> WithFrom(TItem from) => new(from, Into);
}
