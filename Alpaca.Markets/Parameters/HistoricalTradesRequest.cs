namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalTradesClient{TRequest}.GetHistoricalTradesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalTradesRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalTradesRequest, ITrade>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalTradesRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this(new[] { symbol }, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalTradesRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalTradesRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalTradesRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", true)]
    public HistoricalTradesRequest(
        String symbol,
        IInclusiveTimeInterval timeInterval)
        : this(new[] { symbol }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", true)]
    public HistoricalTradesRequest(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval)
        : base(symbols, timeInterval)
    {
    }
    /// <summary>
    /// Gets or sets the feed to pull market data from. The <see cref="MarkedDataFeed.Sip"/> and
    /// <see cref="MarkedDataFeed.Otc"/> are only available to those with a subscription. Default is
    /// <see cref="MarkedDataFeed.Iex"/> for free plans and <see cref="MarkedDataFeed.Sip"/> for paid.
    /// </summary>
    [UsedImplicitly]
    public MarkedDataFeed? Feed { get; set; }

    /// <inheritdoc />
    protected override String LastPathSegment => "trades";

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) => 
        queryBuilder.AddParameter("feed", Feed);

    HistoricalTradesRequest IHistoricalRequest<HistoricalTradesRequest, ITrade>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalTradesRequest(Symbols, this.GetValidatedFrom(), this.GetValidatedInto()) { Feed = Feed }
            .WithPageSize(this.GetPageSize());
}
