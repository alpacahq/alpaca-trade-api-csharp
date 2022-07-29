namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest crypto XBBO request on Alpaca Data API v2.
/// </summary>
[ExcludeFromCodeCoverage]
[Obsolete("This class will be removed in the next major release of SDK.", true)]
public sealed class LatestBestBidOfferRequest : Validation.IRequest
{
    private readonly HashSet<CryptoExchange> _exchanges = new();

    /// <summary>
    /// Creates new instance of <see cref="LatestBestBidOfferRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="exchanges">Crypto exchanges list for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> or <paramref name="exchanges"/> argument is <c>null</c>.
    /// </exception>
    public LatestBestBidOfferRequest(
        String symbol,
        IEnumerable<CryptoExchange> exchanges)
        : this(symbol.EnsureNotNull()) =>
        _exchanges.UnionWith(exchanges.EnsureNotNull());

    /// <summary>
    /// Creates new instance of <see cref="LatestBestBidOfferRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="exchange">Crypto exchange for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public LatestBestBidOfferRequest(
        String symbol,
        CryptoExchange exchange)
        : this(symbol.EnsureNotNull()) =>
        _exchanges.Add(exchange);

    /// <summary>
    /// Creates new instance of <see cref="LatestBestBidOfferRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public LatestBestBidOfferRequest(
        String symbol) =>
        Symbol = symbol.EnsureNotNull();

    /// <summary>
    /// Gets asset symbol for data retrieval.
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
        }.AppendPath($"../../v1beta1/crypto/{Symbol}/xbbo/latest");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbol.TryValidateSymbolName();
    }
}
