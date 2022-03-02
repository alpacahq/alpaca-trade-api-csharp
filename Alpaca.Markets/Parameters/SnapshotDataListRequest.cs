namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for snapshot crypto data requests on Alpaca Data API v2.
/// </summary>
public sealed class SnapshotDataListRequest : Validation.IRequest
{
    private readonly HashSet<String> _symbols = new (StringComparer.Ordinal);

    /// <summary>
    /// Creates new instance of <see cref="SnapshotDataListRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset names for data retrieval.</param>
    /// <param name="exchange">Crypto exchange for data retrieval.</param>
    public SnapshotDataListRequest(
        IEnumerable<String> symbols,
        CryptoExchange exchange)
    {
        _symbols.UnionWith(symbols.EnsureNotNull());
        Exchange = exchange;
    }

    /// <summary>
    /// Gets asset names list for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets crypto exchange for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public CryptoExchange Exchange { get; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("exchange", Exchange.ToEnumString())
                .AddParameter("symbols", Symbols)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath("snapshots");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbols.TryValidateSymbolName();
    }
}
