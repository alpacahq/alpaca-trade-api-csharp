namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalBarsClient{TRequest}.ListHistoricalBarsAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalBarsClient{TRequest}.GetHistoricalBarsAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
public sealed class HistoricalCryptoBarsRequest : HistoricalCryptoRequestBase, IHistoricalRequest<HistoricalCryptoBarsRequest, IBar>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalCryptoBarsRequest(
        String symbol,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : this(new[] { symbol }, from, into, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalCryptoBarsRequest(
        String symbol,
        BarTimeFrame timeFrame,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol }, timeInterval, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : base(symbols, from, into) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval,
        BarTimeFrame timeFrame)
        : base(symbols, timeInterval) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalCryptoBarsRequest(
        String symbol,
        BarTimeFrame timeFrame,
        IInclusiveTimeInterval timeInterval)
        : this(new[] { symbol }, timeInterval, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval,
        BarTimeFrame timeFrame)
        : base(symbols, timeInterval) =>
        TimeFrame = timeFrame;

    private HistoricalCryptoBarsRequest(
        HistoricalCryptoBarsRequest request,
        IEnumerable<CryptoExchange> exchanges)
        : base(request.Symbols, request.TimeInterval,
            request.Exchanges.Concat(exchanges))
    {
        CopyPagination(request.Pagination);
        TimeFrame = request.TimeFrame;
    }

    /// <summary>
    /// Gets type of time bars for retrieval.
    /// </summary>
    [UsedImplicitly]
    public BarTimeFrame TimeFrame { get; }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object
    /// with the updated <see cref="HistoricalCryptoRequestBase.Exchanges"/> list.
    /// </summary>
    /// <param name="exchanges">Crypto exchanges to add into the list.</param>
    /// <returns>The new instance of the <see cref="HistoricalCryptoBarsRequest"/> object.</returns>
    [UsedImplicitly]
    public HistoricalCryptoBarsRequest WithExchanges(
        IEnumerable<CryptoExchange> exchanges) =>
        new(this, exchanges);

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object
    /// with the updated <see cref="HistoricalCryptoRequestBase.Exchanges"/> list.
    /// </summary>
    /// <param name="exchanges">Crypto exchanges to add into the list.</param>
    /// <returns>The new instance of the <see cref="HistoricalCryptoBarsRequest"/> object.</returns>
    [UsedImplicitly]
    public HistoricalCryptoBarsRequest WithExchanges(
        params CryptoExchange[] exchanges) =>
        new(this, exchanges);

    /// <inheritdoc />
    protected override String LastPathSegment => "bars";

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) =>
        base.AddParameters(queryBuilder)
            // ReSharper disable once StringLiteralTypo
            .AddParameter("timeframe", TimeFrame.ToString());

    HistoricalCryptoBarsRequest IHistoricalRequest<HistoricalCryptoBarsRequest, IBar>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalCryptoBarsRequest(Symbols, this.GetValidatedFrom(), this.GetValidatedInto(), TimeFrame)
            .WithPageSize(this.GetPageSize()).WithExchanges(Exchanges);
}
