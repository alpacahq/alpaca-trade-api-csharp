namespace Alpaca.Markets;

internal sealed class AlpacaCryptoDataClient :
    DataHistoricalClientBase<HistoricalCryptoBarsRequest, HistoricalCryptoQuotesRequest, JsonHistoricalCryptoQuote, HistoricalCryptoTradesRequest>,
    IAlpacaCryptoDataClient
{
    public AlpacaCryptoDataClient(
        AlpacaCryptoDataClientConfiguration configuration)
        : base(configuration.EnsureNotNull(nameof(configuration)).GetConfiguredHttpClient())
    {
    }

    public async Task<ITrade> GetLatestTradeAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<ITrade, JsonLatestTrade>(
            await request.EnsureNotNull(nameof(request))
                .GetUriBuilderAsync(HttpClient, "trades").ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public async Task<IQuote> GetLatestQuoteAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IQuote, JsonLatestQuote<JsonHistoricalCryptoQuote>>(
            await request.EnsureNotNull(nameof(request))
                .GetUriBuilderAsync(HttpClient, "quotes").ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);

    public async Task<IQuote> GetLatestBestBidOfferAsync(
        LatestBestBidOfferRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IQuote, JsonLatestBestBidOffer>(
            await request.EnsureNotNull(nameof(request))
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);
}
