namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest stock data requests on Alpaca Data API v2.
/// </summary>
public sealed class LatestMarketDataRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="LatestMarketDataRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public LatestMarketDataRequest(
        String symbol) =>
        Symbol = symbol.EnsureNotNull();

    /// <summary>
    /// Gets asset name for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

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
                .AddParameter("feed", Feed)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath($"{Symbol}/{lastPathSegment}");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbol.TryValidateSymbolName();
    }
}
