namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalBarsClient{TRequest}.ListHistoricalBarsAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalBarsClient{TRequest}.GetHistoricalBarsAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
public sealed class HistoricalBarsRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalBarsRequest, IBar>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalBarsRequest(
        String symbol,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : this(new[] { symbol.EnsureNotNull() }, from, into, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalBarsRequest(
        String symbol,
        BarTimeFrame timeFrame,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol.EnsureNotNull() }, timeInterval, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalBarsRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), from, into) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalBarsRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), timeInterval) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalBarsRequest(
        String symbol,
        BarTimeFrame timeFrame,
        IInclusiveTimeInterval timeInterval)
        : this(new[] { symbol.EnsureNotNull() }, timeInterval, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalBarsRequest(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), timeInterval) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Gets type of time bars for retrieval.
    /// </summary>
    [UsedImplicitly]
    public BarTimeFrame TimeFrame { get; }

    /// <summary>
    /// Gets or sets adjustment type of time bars for retrieval.
    /// </summary>
    [UsedImplicitly]
    public Adjustment? Adjustment { get; set; }

    /// <summary>
    /// Gets or sets the feed to pull market data from. The <see cref="MarkedDataFeed.Sip"/> and
    /// <see cref="MarkedDataFeed.Otc"/> are only available to those with a subscription. Default is
    /// <see cref="MarkedDataFeed.Iex"/> for free plans and <see cref="MarkedDataFeed.Sip"/> for paid.
    /// </summary>
    [UsedImplicitly]
    public MarkedDataFeed? Feed { get; set; }

    /// <inheritdoc />
    protected override String LastPathSegment => "bars";

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) =>
        queryBuilder
            // ReSharper disable once StringLiteralTypo
            .AddParameter("timeframe", TimeFrame.ToString())
            .AddParameter("adjustment", Adjustment)
            .AddParameter("feed", Feed);

    HistoricalBarsRequest IHistoricalRequest<HistoricalBarsRequest, IBar>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalBarsRequest(Symbols, this.GetValidatedFrom(), this.GetValidatedInto(), TimeFrame)
            { Adjustment = Adjustment, Feed = Feed }
            .WithPageSize(this.GetPageSize());
}
