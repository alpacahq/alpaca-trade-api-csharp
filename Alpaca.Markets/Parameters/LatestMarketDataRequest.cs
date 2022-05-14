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
    public LatestMarketDataRequest(
        String symbol) =>
        Symbol = symbol.EnsureNotNull();

    /// <summary>
    /// Gets asset name for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

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
                .AddParameter("feed", Feed)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath($"{Symbol}/{lastPathSegment}");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbol.TryValidateSymbolName();
    }
}
