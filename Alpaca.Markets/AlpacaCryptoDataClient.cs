namespace Alpaca.Markets;

internal sealed class AlpacaCryptoDataClient :
    DataHistoricalClientBase<HistoricalCryptoBarsRequest, HistoricalCryptoQuotesRequest, JsonHistoricalCryptoQuote, HistoricalCryptoTradesRequest>,
    IAlpacaCryptoDataClient
{
    public AlpacaCryptoDataClient(
        AlpacaCryptoDataClientConfiguration configuration)
        : base(configuration.EnsureNotNull().GetConfiguredHttpClient())
    {
    }

    public async Task<ITrade> GetLatestTradeAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<ITrade, JsonLatestTrade>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient, "trades").ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public async Task<IQuote> GetLatestQuoteAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalCryptoQuote>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient, "quotes").ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public async Task<IQuote> GetLatestBestBidOfferAsync(
        LatestBestBidOfferRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IQuote, JsonLatestBestBidOffer>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public async Task<ISnapshot> GetSnapshotAsync(
        SnapshotDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<ISnapshot, JsonCryptoSnapshot>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);
}
