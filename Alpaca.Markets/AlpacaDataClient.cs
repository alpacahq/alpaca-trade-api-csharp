namespace Alpaca.Markets;

internal sealed class AlpacaDataClient :
    DataHistoricalClientBase<HistoricalBarsRequest, HistoricalQuotesRequest, JsonHistoricalQuote, HistoricalTradesRequest>,
    IAlpacaDataClient
{
    public AlpacaDataClient(
        AlpacaDataClientConfiguration configuration)
        : base(configuration.EnsureNotNull().GetConfiguredHttpClient())
    {
    }

    public Task<IBar> GetLatestBarAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<IBar, JsonLatestBar>(
            $"{symbol.EnsureNotNull()}/bars/latest",
            cancellationToken);

    public Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IBar, JsonHistoricalBar>(
            symbols, "bars", _ => _.Bars, cancellationToken);

    public Task<ITrade> GetLatestTradeAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<ITrade, JsonLatestTrade>(
            $"{symbol.EnsureNotNull()}/trades/latest",
            cancellationToken);

    public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ITrade, JsonHistoricalTrade>(
            symbols, "trades", _ => _.Trades, cancellationToken);

    public Task<IQuote> GetLatestQuoteAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalQuote>>(
            $"{symbol.EnsureNotNull()}/quotes/latest",
            cancellationToken);

    public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IQuote, JsonHistoricalQuote>(
            symbols, "quotes", _ => _.Quotes, cancellationToken);

    public Task<ISnapshot> GetSnapshotAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<ISnapshot, JsonSnapshot>(
            $"{symbol.EnsureNotNull()}/snapshot", cancellationToken);

    public async Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
            await getUriBuilder(symbols, "snapshots").ConfigureAwait(false),
            StringComparer.Ordinal, withSymbol<ISnapshot, JsonSnapshot>,
            cancellationToken).ConfigureAwait(false);

    [ExcludeFromCodeCoverage]
    public Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        ListSnapshotsAsync(symbols, cancellationToken);

    public Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
            "meta/exchanges", cancellationToken);

    public Task<IReadOnlyDictionary<String, String>> ListTradeConditionsAsync(
        Tape tape,
        CancellationToken cancellationToken = default) =>
        listConditionsAsync(tape, "trade", cancellationToken);

    public Task<IReadOnlyDictionary<String, String>> ListQuoteConditionsAsync(
        Tape tape,
        CancellationToken cancellationToken = default) =>
        listConditionsAsync(tape, "quote", cancellationToken);
    
    public async Task<IPage<INewsArticle>> ListNewsArticlesAsync(
        NewsArticlesRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IPage<INewsArticle>, JsonNewsPage>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, String>> listConditionsAsync(
        Tape tape,
        String tickType,
        CancellationToken cancellationToken) =>
        await HttpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
            new UriBuilder(HttpClient.BaseAddress!)
            {
                Query = await new QueryBuilder()
                    .AddParameter("tape", tape.ToEnumString())
                    .AsStringAsync().ConfigureAwait(false)
            }.AppendPath($"meta/conditions/{tickType}"),
            cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        IEnumerable<String> symbols,
        String items,
        Func<JsonLatestData, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await HttpClient.GetAsync(
            await getUriBuilder(symbols, $"{items}/latest").ConfigureAwait(false),
            itemsSelector, withSymbol<TApi, TJson>,
            cancellationToken).ConfigureAwait(false);

    private async ValueTask<UriBuilder> getUriBuilder(
        IEnumerable<String> symbols,
        String path) =>
        new UriBuilder(HttpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("symbols", String.Join(",", symbols.EnsureNotNull()))
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath(path);

    private static TApi withSymbol<TApi, TJson>(
        KeyValuePair<String, TJson> kvp)
        where TJson : TApi, ISymbolMutable
    {
        kvp.Value.SetSymbol(kvp.Key);
        return kvp.Value;
    }
}
