namespace Alpaca.Markets;

internal sealed class AlpacaDataClient :
    DataHistoricalClientBase<HistoricalBarsRequest, HistoricalQuotesRequest, JsonHistoricalQuote, HistoricalTradesRequest>,
    IAlpacaDataClient
{
    public AlpacaDataClient(
        AlpacaDataClientConfiguration configuration)
        : base(configuration.EnsureNotNull(nameof(configuration)).GetConfiguredHttpClient())
    {
    }

    public Task<ITrade> GetLatestTradeAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<ITrade, JsonLatestTrade>(
            $"{symbol.EnsureNotNull(nameof(symbol))}/trades/latest",
            cancellationToken);

    public Task<IQuote> GetLatestQuoteAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalQuote>>(
            $"{symbol.EnsureNotNull(nameof(symbol))}/quotes/latest",
            cancellationToken);

    public Task<ISnapshot> GetSnapshotAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<ISnapshot, JsonSnapshot>(
            $"{symbol.EnsureNotNull(nameof(symbol))}/snapshot", cancellationToken);

    public async Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<String, ISnapshot, String, JsonSnapshot>(
            new UriBuilder(HttpClient.BaseAddress!)
            {
                Query = await new QueryBuilder()
                    .AddParameter("symbols", String.Join(",",
                        symbols.EnsureNotNull(nameof(symbols))))
                    .AsStringAsync().ConfigureAwait(false)
            }.AppendPath("snapshots"),
            StringComparer.Ordinal, kvp => kvp.Value.WithSymbol(kvp.Key),
            cancellationToken).ConfigureAwait(false);

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
            await request.EnsureNotNull(nameof(request)).Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, String>> listConditionsAsync(
        Tape tape,
        String tickType,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
            new UriBuilder(HttpClient.BaseAddress!)
            {
                Query = await new QueryBuilder()
                    .AddParameter("tape", tape.ToEnumString())
                    .AsStringAsync().ConfigureAwait(false)
            }.AppendPath($"meta/conditions/{tickType}"),
            cancellationToken).ConfigureAwait(false);
}
