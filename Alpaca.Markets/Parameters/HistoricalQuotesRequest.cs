namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalQuotesClient{TRequest}.GetHistoricalQuotesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalQuotesRequest : HistoricalRequestBase, IHistoricalRequest<HistoricalQuotesRequest, IQuote>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalQuotesRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this(new[] { symbol.EnsureNotNull() }, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalQuotesRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol.EnsureNotNull() }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalQuotesRequest(
        String symbol)
        : this(new[] { symbol.EnsureNotNull() })
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalQuotesRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols.EnsureNotNull(), from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalQuotesRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols.EnsureNotNull(), timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalQuotesRequest(
        IEnumerable<String> symbols)
        : base(symbols.EnsureNotNull(), new Interval<DateTime>())
    {
    }

    /// <summary>
    /// Gets or sets the feed to pull market data from. The <see cref="MarketDataFeed.Sip"/> and
    /// <see cref="MarketDataFeed.Otc"/> are only available to those with a subscription. Default is
    /// <see cref="MarketDataFeed.Iex"/> for free plans and <see cref="MarketDataFeed.Sip"/> for paid.
    /// </summary>
    [UsedImplicitly]
    public MarketDataFeed? Feed { get; set; }

    /// <summary>
    /// Gets or sets the optional parameter for mapping symbol to contract by a specific date.
    /// </summary>
    [UsedImplicitly]
    public DateOnly? UseSymbolAsOfTheDate { get; set; }

    /// <summary>
    /// Gets or sets the optional parameter for the returned prices in ISO 4217 standard.
    /// For example: USD, EUR, JPY, etc. In case of <c>null</c> the default USD will be used.
    /// </summary>
    [UsedImplicitly]
    public String? Currency { get; set; }

    /// <inheritdoc />
    protected override String LastPathSegment => "quotes";

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) => 
        queryBuilder
            .AddParameter("asof", UseSymbolAsOfTheDate)
            .AddParameter("currency", Currency)
            .AddParameter("feed", Feed);

    HistoricalQuotesRequest IHistoricalRequest<HistoricalQuotesRequest, IQuote>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalQuotesRequest(Symbols, TimeInterval)
            {
                Feed = Feed,
                Currency = Currency,
                SortDirection = SortDirection,
                UseSymbolAsOfTheDate = UseSymbolAsOfTheDate
            }
            .WithPageSize(this.GetPageSize());
}
