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
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    public LatestMarketDataListRequest(
        IEnumerable<String> symbols) =>
        _symbols.UnionWith(symbols.EnsureNotNull());

    /// <summary>
    /// Gets asset name for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> Symbols => _symbols;

    /// <summary>
    /// Gets or sets the feed to pull market data from. The <see cref="MarketDataFeed.Sip"/> and
    /// <see cref="MarketDataFeed.Otc"/> are only available to those with a subscription. Default is
    /// <see cref="MarketDataFeed.Iex"/> for free plans and <see cref="MarketDataFeed.Sip"/> for paid.
    /// </summary>
    [UsedImplicitly]
    public MarketDataFeed? Feed { get; set; }

    /// <summary>
    /// Gets or sets the optional parameter for the returned prices in ISO 4217 standard.
    /// For example: USD, EUR, JPY, etc. In case of <c>null</c> the default USD will be used.
    /// </summary>
    [UsedImplicitly]
    public String? Currency { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient,
        String lastPathSegment) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("currency", Currency)
                .AddParameter("symbols", Symbols)
                .AddParameter("feed", Feed)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath(lastPathSegment);

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbols.TryValidateSymbolName();
    }
}
