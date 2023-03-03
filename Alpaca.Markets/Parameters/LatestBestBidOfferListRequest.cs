namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest crypto XBBO request on Alpaca Data API v2.
/// </summary>
[Obsolete("This class will be removed in the next major release of SDK.", false)]
public sealed class LatestBestBidOfferListRequest : Validation.IRequest
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

    private readonly HashSet<CryptoExchange> _exchanges = new();

    /// <summary>
    /// Creates new instance of <see cref="LatestBestBidOfferListRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols list for data retrieval.</param>
    /// <param name="exchanges">Crypto exchanges list for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> or <paramref name="exchanges"/> argument is <c>null</c>.
    /// </exception>
    public LatestBestBidOfferListRequest(
        IEnumerable<String> symbols,
        IEnumerable<CryptoExchange> exchanges)
        : this(symbols.EnsureNotNull()) =>
        _exchanges.UnionWith(exchanges.EnsureNotNull());

    /// <summary>
    /// Creates new instance of <see cref="LatestBestBidOfferListRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols list for data retrieval.</param>
    /// <param name="exchange">Crypto exchange for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public LatestBestBidOfferListRequest(
        IEnumerable<String> symbols,
        CryptoExchange exchange)
        : this(symbols.EnsureNotNull()) =>
        _exchanges.Add(exchange);

    /// <summary>
    /// Creates new instance of <see cref="LatestBestBidOfferListRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbols list for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public LatestBestBidOfferListRequest(
        IEnumerable<String> symbols) =>
        _symbols.UnionWith(symbols.EnsureNotNull());

    /// <summary>
    /// Gets asset symbols for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

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
                .AddParameter("symbols", Symbols)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath("../../../../v1beta1/crypto/xbbos/latest");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbols.TryValidateSymbolName();
    }
}
