namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest crypto data requests on Alpaca Data API v2.
/// </summary>
public sealed class LatestDataListRequest : Validation.IRequest
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);
    /// <summary>
    /// Creates new instance of <see cref="LatestDataListRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset symbol for data retrieval.</param>
    /// <param name="exchange">Crypto exchange for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public LatestDataListRequest(
        IEnumerable<String> symbols,
        CryptoExchange exchange)
    {
        _symbols.UnionWith(symbols.EnsureNotNull());
        Exchange = exchange;
    }

    /// <summary>
    /// Gets asset symbols for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets crypto exchange for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public CryptoExchange Exchange { get; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient,
        String lastPathSegment) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("symbols", Symbols)
                .AddParameter("exchange", Exchange.ToEnumString())
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath($"latest/{lastPathSegment}");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbols.TryValidateSymbolName();
    }
}
