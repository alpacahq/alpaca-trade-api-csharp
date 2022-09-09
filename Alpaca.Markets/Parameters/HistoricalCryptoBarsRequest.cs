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
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        String symbol,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : this(new[] { symbol.EnsureNotNull() }, from, into, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        String symbol,
        BarTimeFrame timeFrame,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol.EnsureNotNull() }, timeInterval, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        String symbol,
        BarTimeFrame timeFrame)
        : this(new[] { symbol.EnsureNotNull() }, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), from, into) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), timeInterval) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), new Interval<DateTime>()) =>
        TimeFrame = timeFrame;

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalCryptoBarsRequest(
        String symbol,
        BarTimeFrame timeFrame,
        IInclusiveTimeInterval timeInterval)
        : this(new[] { symbol.EnsureNotNull() }, timeInterval, timeFrame)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols for data retrieval.</param>
    /// <param name="timeFrame">Type of time bars for retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalCryptoBarsRequest(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval,
        BarTimeFrame timeFrame)
        : base(symbols.EnsureNotNull(), timeInterval) =>
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
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="exchanges"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new instance of the <see cref="HistoricalCryptoBarsRequest"/> object.</returns>
    [UsedImplicitly]
    public HistoricalCryptoBarsRequest WithExchanges(
        IEnumerable<CryptoExchange> exchanges) =>
        new(this, exchanges.EnsureNotNull());

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoBarsRequest"/> object
    /// with the updated <see cref="HistoricalCryptoRequestBase.Exchanges"/> list.
    /// </summary>
    /// <param name="exchanges">Crypto exchanges to add into the list.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="exchanges"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The new instance of the <see cref="HistoricalCryptoBarsRequest"/> object.</returns>
    [UsedImplicitly]
    public HistoricalCryptoBarsRequest WithExchanges(
        params CryptoExchange[] exchanges) =>
        new(this, exchanges.EnsureNotNull());

    /// <inheritdoc />
    protected override String LastPathSegment => "bars";

    internal override QueryBuilder AddParameters(
        QueryBuilder queryBuilder) =>
        base.AddParameters(queryBuilder)
            // ReSharper disable once StringLiteralTypo
            .AddParameter("timeframe", TimeFrame.ToString());

    HistoricalCryptoBarsRequest IHistoricalRequest<HistoricalCryptoBarsRequest, IBar>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalCryptoBarsRequest(Symbols, TimeInterval, TimeFrame)
            .WithPageSize(this.GetPageSize()).WithExchanges(Exchanges);
}
