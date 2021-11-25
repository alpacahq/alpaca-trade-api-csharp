namespace Alpaca.Markets;

/// <summary>
/// Encapsulates implementations of the <see cref="IDateInterval"/> interface and helper methods for it.
/// </summary>
public static class DateInterval
{
    private readonly struct Interval : IDateInterval, IEquatable<Interval>
    {
        internal Interval(
            DateOnly? from,
            DateOnly? into)
        {
            if (from > into)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(from), "Time interval should be valid.");
            }

            From = from;
            Into = into;
        }

        public DateOnly? From { get; }

        public DateOnly? Into { get; }

        public Boolean Equals(Interval other) =>
            Equals((IDateInterval)other);

        public Boolean Equals(IDateInterval? other) =>
            Nullable.Equals(From, other?.From) &&
            Nullable.Equals(Into, other?.Into);

        public override Boolean Equals(Object? obj) =>
            obj is Interval other && Equals(other);

        public override Int32 GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode() * 397) ^ Into.GetHashCode();
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="IInclusiveTimeInterval"/> instance from to the <see cref="IDateInterval"/> instance dates.
    /// </summary>
    /// <param name="interval">Input date interval for converting.</param>
    /// <returns>Inclusive time interval initialized with data from the original date interval.</returns>
    public static IInclusiveTimeInterval AsTimeInterval(
        this IDateInterval interval) =>
        TimeInterval.GetInclusive(
            interval.EnsureNotNull(nameof(interval)).From?.ToDateTime(TimeOnly.MinValue),
            interval.Into?.ToDateTime(TimeOnly.MinValue));

    /// <summary>
    /// Gets boolean flag signals that time interval is empty (both start and end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="IDateInterval.From"/> and <see cref="IDateInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    public static Boolean IsEmpty(
        this IDateInterval interval) =>
        interval.EnsureNotNull(nameof(interval)).Into is null && interval.From is null;

    /// <summary>
    /// Gets boolean flag signals that time interval is open (both start or end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="IDateInterval.From"/> or <see cref="IDateInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    public static Boolean IsOpen(
        this IDateInterval interval) =>
        interval.EnsureNotNull(nameof(interval)).Into is null ^ interval.From is null;

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="from">Starting date for filtering.</param>
    /// <param name="into">Ending date for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    public static TRequest SetDateInterval<TRequest>(
        this TRequest request,
        DateOnly from,
        DateOnly into)
        where TRequest : IRequestWithDateInterval =>
        request.SetDateInterval(new Interval(from, into));

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="interval">Time interval (dates pair) for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    public static TRequest SetDateInterval<TRequest>(
        this TRequest request,
        IDateInterval interval)
        where TRequest : IRequestWithDateInterval
    {
        request.SetInterval(interval);
        return request;
    }

    /// <summary>
    /// Gets inclusive open time interval starting from the <paramref name="from"/> date/time point.
    /// </summary>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    public static IDateInterval GetIntervalFromThat(
        this DateOnly from) =>
        new Interval(from, null);

    /// <summary>
    /// Gets inclusive open time interval ending at the <paramref name="into"/> date/time point.
    /// </summary>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    public static IDateInterval GetIntervalTillThat(
        this DateOnly into) =>
        new Interval(null, into);

    internal static IDateInterval GetInclusive(
        DateOnly? from,
        DateOnly? into) =>
        new Interval(from, into);

    internal static IDateInterval Empty { get; } = new Interval();

    /// <summary>
    /// Deconstructs the <see cref="IDateInterval"/> instance
    /// into two <see cref="Nullable{DateTime}"/> values (tuple).
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="from">Time interval starting point.</param>
    /// <param name="into">Time interval ending point.</param>
    [UsedImplicitly]
    public static void Deconstruct(
        this IDateInterval timeInterval,
        out DateOnly? from,
        out DateOnly? into)
    {
        from = timeInterval.EnsureNotNull(nameof(timeInterval)).From;
        into = timeInterval.Into;
    }

    /// <summary>
    /// Creates new instance of <see cref="IDateInterval"/> object
    /// with the modified <see cref="IDateInterval.Into"/> property value.
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="into">New ending date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IDateInterval"/> object.</returns>
    [UsedImplicitly]
    public static IDateInterval WithInto(
        this IDateInterval timeInterval,
        DateOnly into) =>
        GetInclusive(timeInterval.EnsureNotNull(nameof(timeInterval)).From, into);

    /// <summary>
    /// Creates new instance of <see cref="IDateInterval"/> object
    /// with the modified <see cref="IDateInterval.From"/> property value.
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="from">New starting date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IDateInterval"/> object.</returns>
    [UsedImplicitly]
    public static IDateInterval WithFrom(
        this IDateInterval timeInterval,
        DateOnly from) =>
        GetInclusive(from, timeInterval.EnsureNotNull(nameof(timeInterval)).Into);
}
