namespace Alpaca.Markets;

/// <summary>
/// Encapsulates implementations of the <see cref="ITimeInterval"/> interface and helper methods for it.
/// </summary>
public static class TimeInterval
{
    private readonly record struct Interval :
#pragma warning disable CS0618 // Type or member is obsolete
        IInclusiveTimeInterval, IExclusiveTimeInterval
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private readonly Interval<DateTime> _interval;

        // ReSharper disable once ConvertToPrimaryConstructor
        public Interval(Interval<DateTime> interval) => _interval = interval;

        public DateTime? From => _interval.From;

        public DateTime? Into => _interval.Into;

#pragma warning disable CS0618 // Type or member is obsolete
        public Boolean Equals(IInclusiveTimeInterval? other) =>
#pragma warning restore CS0618 // Type or member is obsolete
            other is Interval interval ? interval.Equals(this) : equals(other?.From, other?.Into);

#pragma warning disable CS0618 // Type or member is obsolete
        public Boolean Equals(IExclusiveTimeInterval? other) =>
#pragma warning restore CS0618 // Type or member is obsolete
            other is Interval interval ? interval.Equals(this) : equals(other?.From, other?.Into);

        public Boolean IsEmpty() => _interval.IsEmpty();

        public Boolean IsOpen() => _interval.IsOpen();

        private Boolean equals(DateTime? from, DateTime? into) => From == from && Into == into;
    }

    /// <summary>
    /// Gets boolean flag signals that time interval is empty (both start and end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> and <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    [Obsolete("Use the IsEmpty() method of Interval<DateTime> structure instead of this one.", false)]
    public static Boolean IsEmpty(this ITimeInterval interval) =>
        interval is Interval wrapper ? wrapper.IsEmpty() : new Interval<DateTime>(interval?.From, interval?.Into).IsEmpty();

    /// <summary>
    /// Gets boolean flag signals that time interval is open (both start or end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> or <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    [Obsolete("Use the IsOpen() method of Interval<DateTime> structure instead of this one.", false)]
    public static Boolean IsOpen(this ITimeInterval interval) =>
        interval is Interval wrapper ? wrapper.IsOpen() : new Interval<DateTime>(interval?.From, interval?.Into).IsOpen();

    /// <summary>
    /// Gets exclusive open time interval ending at the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Ending date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalTillThat() extension method instead of this one.", false)]
    public static IExclusiveTimeInterval GetExclusiveIntervalTillThat(DateTime value) => value.GetIntervalTillThat().wrap();

    /// <summary>
    /// Gets inclusive open time interval ending at the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Ending date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalTillThat() extension method instead of this one.", false)]
    public static IInclusiveTimeInterval GetInclusiveIntervalTillThat(DateTime value) => value.GetIntervalTillThat().wrap();

    /// <summary>
    /// Gets exclusive open time interval starting from the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Starting date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalFromThat() extension method instead of this one.", false)]
    public static IExclusiveTimeInterval GetExclusiveIntervalFromThat(DateTime value) => value.GetIntervalFromThat().wrap();

    /// <summary>
    /// Gets inclusive open time interval starting from the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Starting date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalFromThat() extension method instead of this one.", false)]
    public static IInclusiveTimeInterval GetInclusiveIntervalFromThat(DateTime value) => value.GetIntervalFromThat().wrap();

    /// <summary>
    /// Creates new instance of <see cref="IExclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.Into"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="into">New ending date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IExclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithInto extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IExclusiveTimeInterval WithInto(
        this IExclusiveTimeInterval interval,
        DateTime into) => new Interval<DateTime>(interval.EnsureNotNull(nameof(interval)).From, into).wrap();

    /// <summary>
    /// Creates new instance of <see cref="IInclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.Into"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="into">New ending date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IInclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithInto extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IInclusiveTimeInterval WithInto(
        this IInclusiveTimeInterval interval,
        DateTime into) => new Interval<DateTime>(interval.EnsureNotNull(nameof(interval)).From, into).wrap();

    /// <summary>
    /// Creates new instance of <see cref="IExclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.From"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="from">New starting date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IExclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithFrom extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IExclusiveTimeInterval WithFrom(
        this IExclusiveTimeInterval interval,
        DateTime from) => new Interval<DateTime>(from, interval.EnsureNotNull(nameof(interval)).Into).wrap();

    /// <summary>
    /// Creates new instance of <see cref="IInclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.From"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="from">New starting date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IInclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithFrom extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IInclusiveTimeInterval WithFrom(
        this IInclusiveTimeInterval interval,
        DateTime from) => new Interval<DateTime>(from, interval.EnsureNotNull(nameof(interval)).Into).wrap();

    /// <summary>
    /// Set exclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", false)]
    public static TRequest SetExclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval> =>
        request.SetTimeInterval(new Interval<DateTime>(from, into).wrap());

    /// <summary>
    /// Set exclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="interval">Time interval (date/time pair) for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", false)]
    public static TRequest SetTimeInterval<TRequest>(
        this TRequest request,
        IExclusiveTimeInterval interval)
        where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval>
    {
        request.SetInterval(interval.EnsureNotNull(nameof(interval)));
        return request;
    }

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", false)]
    public static TRequest SetInclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval> =>
        request.SetTimeInterval(new Interval<DateTime>(from, into).wrap());

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="interval">Time interval (date/time pair) for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", false)]
    public static TRequest SetTimeInterval<TRequest>(
        this TRequest request,
        IInclusiveTimeInterval interval)
        where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
    {
        request.SetInterval(interval.EnsureNotNull(nameof(interval)));
        return request;
    }

    /// <summary>
    /// Deconstructs the <see cref="IExclusiveTimeInterval"/> instance
    /// into two <see cref="Nullable{DateTime}"/> values (tuple).
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="from">Time interval starting point.</param>
    /// <param name="into">Time interval ending point.</param>
    [UsedImplicitly]
    [Obsolete("Use the tuple deconstructor of Interval<DateTime> structure instead of this one.", false)]
    public static void Deconstruct(
        IExclusiveTimeInterval interval,
        out DateTime? from,
        out DateTime? into)
    {
        from = interval?.From;
        into = interval?.Into;
    }

    /// <summary>
    /// Deconstructs the <see cref="IInclusiveTimeInterval"/> instance
    /// into two <see cref="Nullable{DateTime}"/> values (tuple).
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="from">Time interval starting point.</param>
    /// <param name="into">Time interval ending point.</param>
    [UsedImplicitly]
    [Obsolete("Use the tuple deconstructor of Interval<DateTime> structure instead of this one.", false)]
    public static void Deconstruct(
        IInclusiveTimeInterval interval,
        out DateTime? from,
        out DateTime? into)
    {
        from = interval?.From;
        into = interval?.Into;
    }

    private static Interval wrap(this Interval<DateTime> interval) => new (interval);
}
