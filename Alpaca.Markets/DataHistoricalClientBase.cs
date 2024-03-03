namespace Alpaca.Markets;

internal abstract class DataHistoricalClientBase<
    THistoricalBarsRequest,
    THistoricalQuotesRequest, THistoricalQuote,
    THistoricalTradesRequest, THistoricalTrade>
    (HttpClient httpClient) : IRateLimitProvider, IDisposable
    where THistoricalQuotesRequest : HistoricalRequestBase, Validation.IRequest
    where THistoricalTradesRequest : HistoricalRequestBase, Validation.IRequest
    where THistoricalBarsRequest : HistoricalRequestBase, Validation.IRequest
    where THistoricalQuote : IQuote, ISymbolMutable
    where THistoricalTrade : ITrade, ISymbolMutable
{
    protected readonly RateLimitHandler RateLimitHandler = new();

    protected readonly HttpClient HttpClient = httpClient;

    public void Dispose()
    {
        HttpClient.Dispose();
        RateLimitHandler.Dispose();
    }

    public IRateLimitValues GetRateLimitValues() => RateLimitHandler.GetCurrent();

    public Task<IPage<IBar>> ListHistoricalBarsAsync(
        THistoricalBarsRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalBarsAsync(request, cancellationToken)
            : getHistoricalBarsAsync(request, cancellationToken).AsPageAsync<IBar, JsonBarsPage>();

    public Task<IMultiPage<IBar>> GetHistoricalBarsAsync(
        THistoricalBarsRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalBarsAsync(request, cancellationToken).AsMultiPageAsync<IBar, JsonMultiBarsPage>()
            : getHistoricalBarsAsync(request, cancellationToken);

    public Task<IPage<IQuote>> ListHistoricalQuotesAsync(
        THistoricalQuotesRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalQuotesAsync(request, cancellationToken)
            : getHistoricalQuotesAsync(request, cancellationToken)
                .AsPageAsync<IQuote, JsonQuotesPage<THistoricalQuote>>();

    public Task<IMultiPage<IQuote>> GetHistoricalQuotesAsync(
        THistoricalQuotesRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalQuotesAsync(request, cancellationToken)
                .AsMultiPageAsync<IQuote, JsonMultiQuotesPage<THistoricalQuote>>()
            : getHistoricalQuotesAsync(request, cancellationToken);

    public Task<IPage<ITrade>> ListHistoricalTradesAsync(
        THistoricalTradesRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalTradesAsync(request, cancellationToken)
            : getHistoricalTradesAsync(request, cancellationToken)
                .AsPageAsync<ITrade, JsonTradesPage<THistoricalTrade>>();

    public Task<IMultiPage<ITrade>> GetHistoricalTradesAsync(
        THistoricalTradesRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalTradesAsync(request, cancellationToken)
                .AsMultiPageAsync<ITrade, JsonMultiTradesPage<THistoricalTrade>>()
            : getHistoricalTradesAsync(request, cancellationToken);

    public Task<IPage<IAuction>> ListHistoricalAuctionsAsync(
        HistoricalAuctionsRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalAuctionsAsync(request, cancellationToken)
            : getHistoricalAuctionsAsync(request, cancellationToken).AsPageAsync<IAuction, JsonAuctionsPage>();

    public Task<IMultiPage<IAuction>> GetHistoricalAuctionsAsync(
        HistoricalAuctionsRequest request,
        CancellationToken cancellationToken = default) =>
        request.HasSingleSymbol
            ? listHistoricalAuctionsAsync(request, cancellationToken).AsMultiPageAsync<IAuction, JsonMultiAuctionsPage>()
            : getHistoricalAuctionsAsync(request, cancellationToken);

    private async Task<IPage<IBar>> listHistoricalBarsAsync(
        THistoricalBarsRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IPage<IBar>, JsonBarsPage>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IMultiPage<IBar>> getHistoricalBarsAsync(
        THistoricalBarsRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IMultiPage<IBar>, JsonMultiBarsPage>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IPage<IQuote>> listHistoricalQuotesAsync(
        THistoricalQuotesRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IPage<IQuote>, JsonQuotesPage<THistoricalQuote>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IMultiPage<IQuote>> getHistoricalQuotesAsync(
        THistoricalQuotesRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IMultiPage<IQuote>, JsonMultiQuotesPage<THistoricalQuote>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IPage<ITrade>> listHistoricalTradesAsync(
        THistoricalTradesRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IPage<ITrade>, JsonTradesPage<THistoricalTrade>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IMultiPage<ITrade>> getHistoricalTradesAsync(
        THistoricalTradesRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IMultiPage<ITrade>, JsonMultiTradesPage<THistoricalTrade>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IPage<IAuction>> listHistoricalAuctionsAsync(
        HistoricalAuctionsRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IPage<IAuction>, JsonAuctionsPage>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async Task<IMultiPage<IAuction>> getHistoricalAuctionsAsync(
        HistoricalAuctionsRequest request,
        CancellationToken cancellationToken = default) =>
        await HttpClient.GetAsync<IMultiPage<IAuction>, JsonMultiAuctionsPage>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(HttpClient).ConfigureAwait(false),
            RateLimitHandler, cancellationToken).ConfigureAwait(false);
}
