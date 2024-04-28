namespace Alpaca.Markets;

using JsonLatestData=JsonLatestData<JsonOptionQuote, JsonOptionTrade, JsonOptionSnapshot>;

internal sealed class AlpacaOptionsDataClient : 
    DataHistoricalClientBase<
        HistoricalOptionBarsRequest,
        HistoricalQuotesRequest, JsonHistoricalQuote,
        HistoricalOptionTradesRequest, JsonOptionTrade>,
    IAlpacaOptionsDataClient
{
    internal AlpacaOptionsDataClient(
        AlpacaOptionsDataClientConfiguration configuration)
#pragma warning disable CA2000
        : base(configuration.EnsureNotNull().GetConfiguredHttpClient())
#pragma warning restore CA2000
    {
    }

    public Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
        CancellationToken cancellationToken = default) =>
        HttpClient.GetAsync<IReadOnlyDictionary<String, String>, Dictionary<String, String>>(
            "meta/exchanges", RateLimitHandler, cancellationToken);

    public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        LatestOptionsDataRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IQuote, JsonOptionQuote>(
            request.EnsureNotNull().Validate(), "quotes/latest", data => data.Quotes, cancellationToken);

    public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        LatestOptionsDataRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ITrade, JsonOptionTrade>(
            request.EnsureNotNull().Validate(), "trades/latest", data => data.Trades, cancellationToken);

    public async Task<IDictionaryPage<IOptionSnapshot>> ListSnapshotsAsync(
        OptionSnapshotRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IDictionaryPage<IOptionSnapshot>, JsonOptionsSnapshotData>(
            await request.EnsureNotNull().Validate().GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    public async Task<IDictionaryPage<IOptionSnapshot>> GetOptionChainAsync(
        OptionChainRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IDictionaryPage<IOptionSnapshot>, JsonOptionsSnapshotData>(
            await request.EnsureNotNull().Validate().GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        LatestOptionsDataRequest request,
        String lastPathSegment,
        Func<JsonLatestData, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await HttpClient.GetAsync(
            await request.GetUriBuilderAsync(HttpClient, lastPathSegment).ConfigureAwait(false),
            itemsSelector, withSymbol<TApi, TJson>, RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private static TApi withSymbol<TApi, TJson>(
        KeyValuePair<String, TJson> kvp)
        where TJson : TApi, ISymbolMutable
    {
        kvp.Value.SetSymbol(kvp.Key);
        return kvp.Value;
    }
}
