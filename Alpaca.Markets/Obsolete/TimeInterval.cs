namespace Alpaca.Markets;

/// <summary>
/// 
/// </summary>
public static class TimeInterval
{
    private readonly record struct Interval :
#pragma warning disable CS0618 // Type or member is obsolete
        ITimeInterval, IInclusiveTimeInterval, IExclusiveTimeInterval
#pragma warning restore CS0618 // Type or member is obsolete
    {
        private readonly Interval<DateTime> _interval;

        public Interval(Interval<DateTime> interval) => _interval = interval;

        public DateTime? From => _interval.From;

        public DateTime? Into => _interval.Into;

#pragma warning disable CS0618 // Type or member is obsolete
        public Boolean Equals(IInclusiveTimeInterval? other) =>
#pragma warning restore CS0618 // Type or member is obsolete
            other is Interval interval ? interval.Equals(this) : Equals(other?.From, other?.Into);

#pragma warning disable CS0618 // Type or member is obsolete
        public Boolean Equals(IExclusiveTimeInterval? other) =>
#pragma warning restore CS0618 // Type or member is obsolete
            other is Interval interval ? interval.Equals(this) : Equals(other?.From, other?.Into);

        private Boolean Equals(DateTime? from, DateTime? into) => From == from && Into == into;

        public Boolean IsEmpty() => _interval.IsEmpty();

        public Boolean IsOpen() => _interval.IsOpen();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    [Obsolete("Use the IsEmpty() method of Interval<DateTime> structure instead of this one.", false)]
    public static Boolean IsEmpty(this ITimeInterval interval) =>
        interval is Interval wrapper ? wrapper.IsEmpty() : new Interval<DateTime>(interval?.From, interval?.Into).IsEmpty();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    [Obsolete("Use the IsOpen() method of Interval<DateTime> structure instead of this one.", false)]
    public static Boolean IsOpen(this ITimeInterval interval) =>
        interval is Interval wrapper ? wrapper.IsOpen() : new Interval<DateTime>(interval?.From, interval?.Into).IsOpen();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("Use the GetIntervalTillThat() extension method instead of this one.", false)]
    public static IExclusiveTimeInterval GetExclusiveIntervalTillThat(DateTime value) => value.GetIntervalTillThat().Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("Use the GetIntervalTillThat() extension method instead of this one.", false)]
    public static IInclusiveTimeInterval GetInclusiveIntervalTillThat(DateTime value) => value.GetIntervalTillThat().Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("Use the GetIntervalFromThat() extension method instead of this one.", false)]
    public static IExclusiveTimeInterval GetExclusiveIntervalFromThat(DateTime value) => value.GetIntervalFromThat().Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    [Obsolete("Use the GetIntervalFromThat() extension method instead of this one.", false)]
    public static IInclusiveTimeInterval GetInclusiveIntervalFromThat(DateTime value) => value.GetIntervalFromThat().Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="into"></param>
    /// <returns></returns>
    [Obsolete("Use the WithInto extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IExclusiveTimeInterval WithInto(
        this IExclusiveTimeInterval interval,
        DateTime into) => new Interval<DateTime>(interval.EnsureNotNull(nameof(interval)).From, into).Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="into"></param>
    /// <returns></returns>
    [Obsolete("Use the WithInto extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IInclusiveTimeInterval WithInto(
        this IInclusiveTimeInterval interval,
        DateTime into) => new Interval<DateTime>(interval.EnsureNotNull(nameof(interval)).From, into).Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    [Obsolete("Use the WithFrom extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IExclusiveTimeInterval WithFrom(
        this IExclusiveTimeInterval interval,
        DateTime from) => new Interval<DateTime>(from, interval.EnsureNotNull(nameof(interval)).Into).Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    [Obsolete("Use the WithFrom extension method of Interval<DateTime> structure instead of this one.", false)]
    public static IInclusiveTimeInterval WithFrom(
        this IInclusiveTimeInterval interval,
        DateTime from) => new Interval<DateTime>(from, interval.EnsureNotNull(nameof(interval)).Into).Wrap();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="from"></param>
    /// <param name="into"></param>
    /// <returns></returns>
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", false)]
    public static TRequest SetExclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IExclusiveTimeInterval> =>
        request.SetTimeInterval(new Interval<DateTime>(from, into).Wrap());

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="from"></param>
    /// <param name="into"></param>
    /// <returns></returns>
    [Obsolete("Use WithInterval method of the requests type directly instead of this extensions method.", false)]
    public static TRequest SetInclusiveTimeInterval<TRequest>(
        this TRequest request,
        DateTime from,
        DateTime into)
        where TRequest : IRequestWithTimeInterval<IInclusiveTimeInterval> =>
        request.SetTimeInterval(new Interval<DateTime>(from, into).Wrap());

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
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
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="from"></param>
    /// <param name="into"></param>
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
    /// 
    /// </summary>
    /// <param name="interval"></param>
    /// <param name="from"></param>
    /// <param name="into"></param>
    [Obsolete("Use the tuple deconstructor of Interval<DateTime> structure instead of this one.", false)]
    public static void Deconstruct(
        IInclusiveTimeInterval interval,
        out DateTime? from,
        out DateTime? into)
    {
        from = interval?.From;
        into = interval?.Into;
    }

    private static Interval Wrap(this Interval<DateTime> interval) => new (interval);
}
