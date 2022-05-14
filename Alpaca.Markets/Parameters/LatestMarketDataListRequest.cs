namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest stock data requests on Alpaca Data API v2.
/// </summary>
public sealed class LatestMarketDataListRequest : Validation.IRequest
{
    private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

    /// <summary>
    /// Creates new instance of <see cref="LatestMarketDataListRequest"/> object.
    /// </summary>
    /// <param name="symbols">Asset name for data retrieval.</param>
    public LatestMarketDataListRequest(
        IEnumerable<String> symbols) =>
        _symbols.UnionWith(symbols.EnsureNotNull());

    /// <summary>
    /// Gets asset name for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets or sets the feed to pull market data from. The <see cref="MarkedDataFeed.Sip"/> and
    /// <see cref="MarkedDataFeed.Otc"/> are only available to those with a subscription. Default is
    /// <see cref="MarkedDataFeed.Iex"/> for free plans and <see cref="MarkedDataFeed.Sip"/> for paid.
    /// </summary>
    [UsedImplicitly]
    public MarkedDataFeed? Feed { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient,
        String lastPathSegment) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("symbols", Symbols)
                .AddParameter("feed", Feed)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath(lastPathSegment);

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbols.TryValidateSymbolName();
    }
}
