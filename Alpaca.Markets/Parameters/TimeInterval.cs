namespace Alpaca.Markets;

/// <summary>
/// Encapsulates implementations of the <see cref="ITimeInterval"/> interface and helper methods for it.
/// </summary>
public static class TimeInterval
{
    private readonly struct Interval :
        IInclusiveTimeInterval,
        IExclusiveTimeInterval,
        IEquatable<Interval>
    {
        internal Interval(
            DateTime? from,
            DateTime? into)
        {
            if (from > into)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(from), "Time interval should be valid.");
            }

            From = from;
            Into = into;
        }

        public DateTime? From { get; }

        public DateTime? Into { get; }

        public Boolean Equals(Interval other) =>
            Equals((IInclusiveTimeInterval)other);

        public Boolean Equals(IInclusiveTimeInterval? other) =>
            Nullable.Equals(From, other?.From) &&
            Nullable.Equals(Into, other?.Into);

        public Boolean Equals(IExclusiveTimeInterval? other) =>
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
    /// Gets the <see cref="IDateInterval"/> instance from to the <see cref="IInclusiveTimeInterval"/> instance dates.
    /// </summary>
    /// <param name="interval">Input inclusive time interval for converting.</param>
    /// <returns>Date interval initialized with data from the original inclusive time interval.</returns>
    public static IDateInterval AsDateInterval(
        this IInclusiveTimeInterval interval) =>
        DateInterval.GetInclusive(
            interval.EnsureNotNull(nameof(interval)).From.AsDateOnly(),
            interval.Into.AsDateOnly());

    /// <summary>
    /// Gets boolean flag signals that time interval is empty (both start and end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> and <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    public static Boolean IsEmpty(
        this ITimeInterval interval) =>
        interval.EnsureNotNull(nameof(interval)).Into is null && interval.From is null;

    /// <summary>
    /// Gets boolean flag signals that time interval is open (both start or end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> or <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    public static Boolean IsOpen(
        this ITimeInterval interval) =>
        interval.EnsureNotNull(nameof(interval)).Into is null ^ interval.From is null;

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    public static TRequest SetInclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval> =>
        request.SetTimeInterval(new Interval(from, into));

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="interval">Time interval (date/time pair) for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    public static TRequest SetTimeInterval<TRequest>(
        this TRequest request,
        IInclusiveTimeInterval interval)
        where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
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
    public static IInclusiveTimeInterval GetInclusiveIntervalFromThat(
        this DateTime from) =>
        new Interval(from, null);

    /// <summary>
    /// Gets inclusive open time interval ending at the <paramref name="into"/> date/time point.
    /// </summary>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    public static IInclusiveTimeInterval GetInclusiveIntervalTillThat(
        this DateTime into) =>
        new Interval(null, into);

    /// <summary>
    /// Set exclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    public static TRequest SetExclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval> =>
        request.SetTimeInterval(new Interval(from, into));

    /// <summary>
    /// Set exclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="interval">Time interval (date/time pair) for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    public static TRequest SetTimeInterval<TRequest>(
        this TRequest request,
        IExclusiveTimeInterval interval)
        where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval>
    {
        request.SetInterval(interval);
        return request;
    }

    /// <summary>
    /// Gets exclusive open time interval starting from the <paramref name="from"/> date/time point.
    /// </summary>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <returns>Exclusive open time interval.</returns>
    [UsedImplicitly]
    public static IExclusiveTimeInterval GetExclusiveIntervalFromThat(
        this DateTime from) =>
        new Interval(from, null);

    /// <summary>
    /// Gets exclusive open time interval ending at the <paramref name="into"/> date/time point.
    /// </summary>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Exclusive open time interval.</returns>
    [UsedImplicitly]
    public static IExclusiveTimeInterval GetExclusiveIntervalTillThat(
        this DateTime into) =>
        new Interval(null, into);

    internal static IInclusiveTimeInterval GetInclusive(
        DateTime? from,
        DateTime? into) =>
        new Interval(from, into);

    [UsedImplicitly]
    internal static IExclusiveTimeInterval GetExclusive(
        DateTime? from,
        DateTime? into) =>
        new Interval(from, into);

    internal static IInclusiveTimeInterval InclusiveEmpty { get; } = new Interval();

    internal static IExclusiveTimeInterval ExclusiveEmpty { get; } = new Interval();

    /// <summary>
    /// Deconstructs the <see cref="IInclusiveTimeInterval"/> instance
    /// into two <see cref="Nullable{DateTime}"/> values (tuple).
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="from">Time interval starting point.</param>
    /// <param name="into">Time interval ending point.</param>
    [UsedImplicitly]
    public static void Deconstruct(
        this IInclusiveTimeInterval timeInterval,
        out DateTime? from,
        out DateTime? into)
    {
        from = timeInterval.EnsureNotNull(nameof(timeInterval)).From;
        into = timeInterval.Into;
    }

    /// <summary>
    /// Deconstructs the <see cref="IExclusiveTimeInterval"/> instance
    /// into two <see cref="Nullable{DateTime}"/> values (tuple).
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="from">Time interval starting point.</param>
    /// <param name="into">Time interval ending point.</param>
    [UsedImplicitly]
    public static void Deconstruct(
        this IExclusiveTimeInterval timeInterval,
        out DateTime? from,
        out DateTime? into)
    {
        from = timeInterval.EnsureNotNull(nameof(timeInterval)).From;
        into = timeInterval.Into;
    }

    /// <summary>
    /// Creates new instance of <see cref="IInclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.Into"/> property value.
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="into">New ending date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IInclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    public static IInclusiveTimeInterval WithInto(
        this IInclusiveTimeInterval timeInterval,
        DateTime into) =>
        GetInclusive(timeInterval.EnsureNotNull(nameof(timeInterval)).From, into);

    /// <summary>
    /// Creates new instance of <see cref="IExclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.Into"/> property value.
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="into">New ending date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IExclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    public static IExclusiveTimeInterval WithInto(
        this IExclusiveTimeInterval timeInterval,
        DateTime into) =>
        GetExclusive(timeInterval.EnsureNotNull(nameof(timeInterval)).From, into);

    /// <summary>
    /// Creates new instance of <see cref="IInclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.From"/> property value.
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="from">New starting date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IInclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    public static IInclusiveTimeInterval WithFrom(
        this IInclusiveTimeInterval timeInterval,
        DateTime from) =>
        GetInclusive(from, timeInterval.EnsureNotNull(nameof(timeInterval)).Into);

    /// <summary>
    /// Creates new instance of <see cref="IExclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.From"/> property value.
    /// </summary>
    /// <param name="timeInterval">Original time interval.</param>
    /// <param name="from">New starting date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IExclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    public static IExclusiveTimeInterval WithFrom(
        this IExclusiveTimeInterval timeInterval,
        DateTime from) =>
        GetExclusive(from, timeInterval.EnsureNotNull(nameof(timeInterval)).Into);
}
