namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest crypto XBBO request on Alpaca Data API v2.
/// </summary>
public sealed class LatestBestBidOfferRequest : Validation.IRequest
{
    private readonly HashSet<CryptoExchange> _exchanges = new();

    /// <summary>
    /// Creates new instance of <see cref="LatestDataRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="exchanges">Crypto exchanges list for data retrieval.</param>
    public LatestBestBidOfferRequest(
        String symbol,
        IEnumerable<CryptoExchange> exchanges)
        : this(symbol) =>
        _exchanges.UnionWith(exchanges);

    /// <summary>
    /// Creates new instance of <see cref="LatestDataRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="exchange">Crypto exchange for data retrieval.</param>
    public LatestBestBidOfferRequest(
        String symbol,
        CryptoExchange exchange)
        : this(symbol) =>
        _exchanges.Add(exchange);

    /// <summary>
    /// Creates new instance of <see cref="LatestDataRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    public LatestBestBidOfferRequest(
        String symbol) =>
        Symbol = symbol.EnsureNotNull();

    /// <summary>
    /// Gets asset name for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

    /// <summary>
    /// Gets crypto exchanges list for data retrieval (empty list means 'all exchanges').
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<CryptoExchange> Exchanges => _exchanges;

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("exchanges", Exchanges)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath($"{Symbol}/xbbo/latest");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbol.TryValidateSymbolName();
    }
}
