namespace Alpaca.Markets;

using JsonLatestData=JsonLatestData<JsonOptionQuote, JsonOptionTrade, JsonOptionSnapshot>;

internal sealed class AlpacaOptionsDataClient : IAlpacaOptionsDataClient
    //where THistoricalQuotesRequest : HistoricalRequestBase, Validation.IRequest
    //where THistoricalTradesRequest : HistoricalRequestBase, Validation.IRequest
    //where THistoricalBarsRequest : HistoricalRequestBase, Validation.IRequest
    //where THistoricalQuote : IQuote, ISymbolMutable
{
    private readonly RateLimitHandler _rateLimitHandler = new();

    private readonly HttpClient _httpClient;

    internal AlpacaOptionsDataClient(
        AlpacaOptionsDataClientConfiguration configuration) =>
        _httpClient = configuration.EnsureNotNull().GetConfiguredHttpClient();

    public void Dispose()
    {
        _httpClient.Dispose();
        _rateLimitHandler.Dispose();
    }

    public IRateLimitValues GetRateLimitValues() => _rateLimitHandler.GetCurrent();
    public Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
            "meta/exchanges", _rateLimitHandler, cancellationToken);

    public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IQuote, JsonOptionQuote>(
            symbols.EnsureNotNull(), "quotes/latest", data => data.Quotes, cancellationToken);

    public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ITrade, JsonOptionTrade>(
            symbols.EnsureNotNull(), "trades/latest", data => data.Trades, cancellationToken);

    public Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ISnapshot, JsonOptionSnapshot>(
            symbols.EnsureNotNull(), "snapshots", data => data.Snapshots, cancellationToken);

    public Task<IReadOnlyDictionary<String, ISnapshot>> GetOptionChainAsync(
        String underlyingSymbol,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ISnapshot, JsonOptionSnapshot>(
            [], $"snapshots/{underlyingSymbol.EnsureNotNull()}", data => data.Snapshots, cancellationToken);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        IEnumerable<String> symbols,
        String lastPathSegment,
        Func<JsonLatestData, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await _httpClient.GetAsync(
            await getUriBuilderAsync(symbols, lastPathSegment).ConfigureAwait(false),
            itemsSelector, withSymbol<TApi, TJson>,
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async ValueTask<UriBuilder> getUriBuilderAsync(
        IEnumerable<String> symbols,
        String lastPathSegment) =>
        new UriBuilder(_httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("symbols", symbols.ToList())
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath(lastPathSegment);

    private static TApi withSymbol<TApi, TJson>(
        KeyValuePair<String, TJson> kvp)
        where TJson : TApi, ISymbolMutable
    {
        kvp.Value.SetSymbol(kvp.Key);
        return kvp.Value;
    }
}
