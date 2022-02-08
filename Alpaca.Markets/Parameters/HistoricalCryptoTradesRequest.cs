namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync(TRequest,CancellationToken)"/> and
/// <see cref="IHistoricalTradesClient{TRequest}.GetHistoricalTradesAsync(TRequest,CancellationToken)"/> calls.
/// </summary>
[UsedImplicitly]
public sealed class HistoricalCryptoTradesRequest : HistoricalCryptoRequestBase, IHistoricalRequest<HistoricalCryptoTradesRequest, ITrade>
{
    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalCryptoTradesRequest(
        String symbol,
        DateTime from,
        DateTime into)
        : this(new[] { symbol }, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalCryptoTradesRequest(
        String symbol,
        Interval<DateTime> timeInterval)
        : this(new[] { symbol }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset names for data retrieval.</param>
    /// <param name="from">Filter data equal to or after this time.</param>
    /// <param name="into">Filter data equal to or before this time.</param>
    public HistoricalCryptoTradesRequest(
        IEnumerable<String> symbols,
        DateTime from,
        DateTime into)
        : base(symbols, from, into)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset names for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    public HistoricalCryptoTradesRequest(
        IEnumerable<String> symbols,
        Interval<DateTime> timeInterval)
        : base(symbols, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalCryptoTradesRequest(
        String symbol,
        IInclusiveTimeInterval timeInterval)
        : this(new[] { symbol }, timeInterval)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset names for data retrieval.</param>
    /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
    [ExcludeFromCodeCoverage]
    [Obsolete("Use constructor with Interval<DateTime> argument instead of this one.", false)]
    public HistoricalCryptoTradesRequest(
        IEnumerable<String> symbols,
        IInclusiveTimeInterval timeInterval)
        : base(symbols, timeInterval)
    {
    }

    private HistoricalCryptoTradesRequest(
        HistoricalCryptoTradesRequest request,
        IEnumerable<CryptoExchange> exchanges)
        : base(request.Symbols, request.TimeInterval,
            request.Exchanges.Concat(exchanges)) =>
        CopyPagination(request.Pagination);

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object
    /// with the updated <see cref="HistoricalCryptoRequestBase.Exchanges"/> list.
    /// </summary>
    /// <param name="exchanges">Crypto exchanges to add into the list.</param>
    /// <returns>The new instance of the <see cref="HistoricalCryptoTradesRequest"/> object.</returns>
    [UsedImplicitly]
    public HistoricalCryptoTradesRequest WithExchanges(
        IEnumerable<CryptoExchange> exchanges) =>
        new(this, exchanges);

    /// <summary>
    /// Creates new instance of <see cref="HistoricalCryptoTradesRequest"/> object
    /// with the updated <see cref="HistoricalCryptoRequestBase.Exchanges"/> list.
    /// </summary>
    /// <param name="exchanges">Crypto exchanges to add into the list.</param>
    /// <returns>The new instance of the <see cref="HistoricalCryptoTradesRequest"/> object.</returns>
    [UsedImplicitly]
    public HistoricalCryptoTradesRequest WithExchanges(
        params CryptoExchange[] exchanges) =>
        new(this, exchanges);

    /// <inheritdoc />
    protected override String LastPathSegment => "trades";

    HistoricalCryptoTradesRequest IHistoricalRequest<HistoricalCryptoTradesRequest, ITrade>.GetValidatedRequestWithoutPageToken() =>
        new HistoricalCryptoTradesRequest(Symbols, this.GetValidatedFrom(), this.GetValidatedInto())
            .WithPageSize(this.GetPageSize()).WithExchanges(Exchanges);
}
