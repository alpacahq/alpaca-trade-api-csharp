namespace Alpaca.Markets;

internal sealed class AlpacaCryptoDataClient :
    DataHistoricalClientBase<HistoricalCryptoBarsRequest, HistoricalCryptoQuotesRequest, JsonHistoricalCryptoQuote, HistoricalCryptoTradesRequest>,
    IAlpacaCryptoDataClient
{
    internal AlpacaCryptoDataClient(
        AlpacaCryptoDataClientConfiguration configuration)
        : base(configuration.EnsureNotNull().GetConfiguredHttpClient())
    {
    }

    [Obsolete("This method will be removed in the next major release of SDK. Use the ListLatestBarsAsync method instead.", false)]
    public async Task<IBar> GetLatestBarAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IBar, JsonLatestBar>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient, "bars").ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IBar, JsonHistoricalBar>(
            request, "bars", _ => _.Bars, cancellationToken);

    [Obsolete("This method will be removed in the next major release of SDK. Use the ListLatestTradesAsync method instead.", false)]
    public async Task<ITrade> GetLatestTradeAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<ITrade, JsonLatestTrade>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient, "trades").ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<ITrade, JsonHistoricalTrade>(
            request, "trades", _ => _.Trades, cancellationToken);

    [Obsolete("This method will be removed in the next major release of SDK. Use the ListLatestQuotesAsync method instead.", false)]
    public async Task<IQuote> GetLatestQuoteAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalCryptoQuote>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient, "quotes").ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default) =>
        getLatestAsync<IQuote, JsonHistoricalCryptoQuote>(
            request, "quotes", _ => _.Quotes, cancellationToken);

    [Obsolete("This method will be removed in the next major release of SDK.", false)]
    public async Task<IQuote> GetLatestBestBidOfferAsync(
        LatestBestBidOfferRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IQuote, JsonLatestBestBidOffer>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    [Obsolete("This method will be removed in the next major release of SDK.", false)]
    public async Task<IReadOnlyDictionary<String, IQuote>> ListLatestBestBidOffersAsync(
        LatestBestBidOfferListRequest request,
        CancellationToken cancellationToken = default) =>
        await getLatestAsync<IQuote, JsonHistoricalCryptoQuote>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            _ => _.LatestBestBidOffers, cancellationToken).ConfigureAwait(false);

    [Obsolete("This method will be removed in the next major release of SDK. Use the ListSnapshotsAsync method instead.", false)]
    public async Task<ISnapshot> GetSnapshotAsync(
        SnapshotDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<ISnapshot, JsonCryptoSnapshot>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        SnapshotDataListRequest request,
        CancellationToken cancellationToken = default) =>
        await getLatestAsync<ISnapshot, JsonCryptoSnapshot>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            _ => _.Snapshots, cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyDictionary<String, IOrderBook>> ListLatestOrderBooksAsync(
        LatestOrderBooksRequest request,
        CancellationToken cancellationToken = default) =>
        await getLatestAsync<IOrderBook, JsonHistoricalOrderBook>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            _ => _.OrderBooks, cancellationToken).ConfigureAwait(false);

    public Task<IMarketMovers> GetTopMarketMoversAsync(
        Int32? numberOfLosersAndGainersInResponse = default,
        CancellationToken cancellationToken = default) =>
        HttpClient.GetTopMarketMoversAsync(
            "crypto", numberOfLosersAndGainersInResponse, cancellationToken);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        LatestDataListRequest request,
        String items,
        Func<JsonLatestData<JsonHistoricalCryptoQuote>, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await getLatestAsync<TApi, TJson>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient, items).ConfigureAwait(false),
            itemsSelector, cancellationToken).ConfigureAwait(false);

    private async Task<IReadOnlyDictionary<String, TApi>> getLatestAsync<TApi, TJson>(
        UriBuilder uriBuilder,
        Func<JsonLatestData<JsonHistoricalCryptoQuote>, Dictionary<String, TJson>> itemsSelector,
        CancellationToken cancellationToken)
        where TJson : TApi, ISymbolMutable =>
        await HttpClient.GetAsync(
            uriBuilder, itemsSelector, withSymbol<TApi, TJson>,
            cancellationToken).ConfigureAwait(false);

    private static TApi withSymbol<TApi, TJson>(
        KeyValuePair<String, TJson> kvp)
        where TJson : TApi, ISymbolMutable
    {
        kvp.Value.SetSymbol(kvp.Key);
        return kvp.Value;
    }
}
