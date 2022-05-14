namespace Alpaca.Markets;

/// <summary>
/// Encapsulates implementations of the <see cref="ITimeInterval"/> interface and helper methods for it.
/// </summary>
[ExcludeFromCodeCoverage]
public static class TimeInterval
{
    /// <summary>
    /// Gets boolean flag signals that time interval is empty (both start and end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> and <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    [Obsolete("Use the IsEmpty() method of Interval<DateTime> structure instead of this one.", true)]
    public static Boolean IsEmpty(this ITimeInterval interval) =>
        new Interval<DateTime>(interval.EnsureNotNull().From, interval.EnsureNotNull().Into).IsEmpty();

    /// <summary>
    /// Gets boolean flag signals that time interval is open (both start or end date equal to <c>null</c>).
    /// </summary>
    /// <param name="interval">Target time interval for checking.</param>
    /// <returns>
    /// Returns <c>true</c> if both <see cref="ITimeInterval.From"/> or <see cref="ITimeInterval.Into"/> equal to <c>null</c>.
    /// </returns>
    [UsedImplicitly]
    [Obsolete("Use the IsOpen() method of Interval<DateTime> structure instead of this one.", true)]
    public static Boolean IsOpen(this ITimeInterval interval) =>
        new Interval<DateTime>(interval.EnsureNotNull().From, interval.EnsureNotNull().Into).IsOpen();

    /// <summary>
    /// Gets exclusive open time interval ending at the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Ending date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalTillThat() extension method instead of this one.", true)]
    public static IExclusiveTimeInterval GetExclusiveIntervalTillThat(DateTime value) =>
        throw new InvalidOperationException("Use the GetIntervalTillThat() extension method.");

    /// <summary>
    /// Gets inclusive open time interval ending at the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Ending date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalTillThat() extension method instead of this one.", true)]
    public static IInclusiveTimeInterval GetInclusiveIntervalTillThat(DateTime value) =>
        throw new InvalidOperationException("Use the GetIntervalTillThat() extension method.");

    /// <summary>
    /// Gets exclusive open time interval starting from the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Starting date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalFromThat() extension method instead of this one.", true)]
    public static IExclusiveTimeInterval GetExclusiveIntervalFromThat(DateTime value) =>
        throw new InvalidOperationException("Use the GetIntervalFromThat() extension method.");

    /// <summary>
    /// Gets inclusive open time interval starting from the <paramref name="value"/> date/time point.
    /// </summary>
    /// <param name="value">Starting date/time point for filtering.</param>
    /// <returns>Inclusive open time interval.</returns>
    [UsedImplicitly]
    [Obsolete("Use the GetIntervalFromThat() extension method instead of this one.", true)]
    public static IInclusiveTimeInterval GetInclusiveIntervalFromThat(DateTime value) =>
        throw new InvalidOperationException("Use the GetIntervalFromThat() extension method.");

    /// <summary>
    /// Creates new instance of <see cref="IExclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.Into"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="into">New ending date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IExclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithInto extension method of Interval<DateTime> structure instead of this one.", true)]
    public static IExclusiveTimeInterval WithInto(
        this IExclusiveTimeInterval interval,
        DateTime into) =>
        throw new InvalidOperationException("Use the WithInto extension method of Interval<DateTime> structure.");

    /// <summary>
    /// Creates new instance of <see cref="IInclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.Into"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="into">New ending date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IInclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithInto extension method of Interval<DateTime> structure instead of this one.", true)]
    public static IInclusiveTimeInterval WithInto(
        this IInclusiveTimeInterval interval,
        DateTime into) =>
        throw new InvalidOperationException("Use the WithInto extension method of Interval<DateTime> structure.");

    /// <summary>
    /// Creates new instance of <see cref="IExclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.From"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="from">New starting date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IExclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithFrom extension method of Interval<DateTime> structure instead of this one.", true)]
    public static IExclusiveTimeInterval WithFrom(
        this IExclusiveTimeInterval interval,
        DateTime from) =>
        throw new InvalidOperationException("Use the WithFrom extension method of Interval<DateTime> structure.");

    /// <summary>
    /// Creates new instance of <see cref="IInclusiveTimeInterval"/> object
    /// with the modified <see cref="ITimeInterval.From"/> property value.
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="from">New starting date/time point for interval.</param>
    /// <returns>The new instance of <see cref="IInclusiveTimeInterval"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use the WithFrom extension method of Interval<DateTime> structure instead of this one.", true)]
    public static IInclusiveTimeInterval WithFrom(
        this IInclusiveTimeInterval interval,
        DateTime from) =>
        throw new InvalidOperationException("Use the WithFrom extension method of Interval<DateTime> structure.");

    /// <summary>
    /// Set exclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="interval">Time interval (date/time pair) for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", true)]
    public static TRequest SetTimeInterval<TRequest>(
        this TRequest request,
        IExclusiveTimeInterval interval)
        where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval>
    {
        request.SetInterval(interval.EnsureNotNull());
        return request;
    }

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="interval">Time interval (date/time pair) for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", true)]
    public static TRequest SetTimeInterval<TRequest>(
        this TRequest request,
        IInclusiveTimeInterval interval)
        where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
    {
        request.SetInterval(interval.EnsureNotNull());
        return request;
    }

    /// <summary>
    /// Set exclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", true)]
    public static TRequest SetExclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval> =>
        throw new InvalidOperationException("Use WithInterval method of the requests type directly.");

    /// <summary>
    /// Set inclusive time interval for <paramref name="request"/> object.
    /// </summary>
    /// <param name="request">Target request for setting filtering interval.</param>
    /// <param name="from">Starting date/time point for filtering.</param>
    /// <param name="into">Ending date/time point for filtering.</param>
    /// <returns>Fluent interface - returns <paramref name="request"/> object.</returns>
    [UsedImplicitly]
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", true)]
    public static TRequest SetInclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval> =>
        throw new InvalidOperationException("Use WithInterval method of the requests type directly.");

    /// <summary>
    /// Deconstructs the <see cref="IExclusiveTimeInterval"/> instance
    /// into two <see cref="Nullable{DateTime}"/> values (tuple).
    /// </summary>
    /// <param name="interval">Original time interval.</param>
    /// <param name="from">Time interval starting point.</param>
    /// <param name="into">Time interval ending point.</param>
    [UsedImplicitly]
    [Obsolete("Use the tuple deconstructor of Interval<DateTime> structure instead of this one.", true)]
    public static void Deconstruct(
        IExclusiveTimeInterval interval,
        out DateTime? from,
        out DateTime? into)
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
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
    [Obsolete("Use the tuple deconstructor of Interval<DateTime> structure instead of this one.", true)]
    public static void Deconstruct(
        IInclusiveTimeInterval interval,
        out DateTime? from,
        out DateTime? into)
    {
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        from = interval?.From;
        into = interval?.Into;
    }

    [Obsolete("Used only for reducing code duplication.", true)]
    internal static Interval<DateTime> AsDateTimeInterval(
        this ITimeInterval interval) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        new (interval?.From, interval?.Into);

    [Obsolete("Used only for reducing code duplication.", true)]
    internal static Interval<DateOnly> AsDateOnlyInterval(
        this ITimeInterval interval) =>
        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        new (interval?.From.AsDateOnly(), interval?.Into.AsDateOnly());
}
