namespace Alpaca.Markets;

internal sealed partial class AlpacaTradingClient
{
    public Task<IReadOnlyList<IWatchList>> ListWatchListsAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IReadOnlyList<IWatchList>, List<JsonWatchList>>(
            "v2/watchlists", _rateLimitHandler, cancellationToken);

    public Task<IWatchList> CreateWatchListAsync(
        NewWatchListRequest request,
        CancellationToken cancellationToken = default) =>
        _httpClient.PostAsync<IWatchList, JsonWatchList, NewWatchListRequest>(
            "v2/watchlists", request,  _rateLimitHandler, cancellationToken);

    public Task<IWatchList> GetWatchListByIdAsync(
        Guid watchListId,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IWatchList, JsonWatchList>(
            getEndpointUri(watchListId), _rateLimitHandler, cancellationToken);

    public async Task<IWatchList> GetWatchListByNameAsync(
        String name,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IWatchList, JsonWatchList>(
            await getEndpointUriBuilderAsync(name).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IWatchList> UpdateWatchListByIdAsync(
        UpdateWatchListRequest request,
        CancellationToken cancellationToken = default) =>
        _httpClient.PutAsync<IWatchList, JsonWatchList, UpdateWatchListRequest>(
            getEndpointUri(request.EnsureNotNull().Validate().WatchListId), request,
            _rateLimitHandler, cancellationToken);

    public Task<IWatchList> AddAssetIntoWatchListByIdAsync(
        ChangeWatchListRequest<Guid> request,
        CancellationToken cancellationToken = default) =>
        _httpClient.PostAsync<IWatchList, JsonWatchList, ChangeWatchListRequest<Guid>>(
            getEndpointUri(request.EnsureNotNull().Validate().Key), request,
            _rateLimitHandler, cancellationToken);

    public async Task<IWatchList> AddAssetIntoWatchListByNameAsync(
        ChangeWatchListRequest<String> request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.PostAsync<IWatchList, JsonWatchList, ChangeWatchListRequest<String>>(
            await getEndpointUriBuilderAsync(request.EnsureNotNull().Validate().Key).ConfigureAwait(false), request,
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
        ChangeWatchListRequest<Guid> request,
        CancellationToken cancellationToken = default) =>
        _httpClient.DeleteAsync<IWatchList, JsonWatchList>(
            getEndpointUri(request.EnsureNotNull().Validate().Key, request.Asset),
            _rateLimitHandler, cancellationToken);

    public async Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
        ChangeWatchListRequest<String> request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.DeleteAsync<IWatchList, JsonWatchList>(
            await getEndpointUriBuilderAsync(
                    request.EnsureNotNull().Validate().Key, request.Asset)
                .ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<Boolean> DeleteWatchListByIdAsync(
        Guid watchListId,
        CancellationToken cancellationToken = default) =>
        _httpClient.TryDeleteAsync(
            getEndpointUri(watchListId), _rateLimitHandler, cancellationToken);

    public async Task<Boolean> DeleteWatchListByNameAsync(
        String name,
        CancellationToken cancellationToken = default) =>
        await _httpClient.TryDeleteAsync(
            await getEndpointUriBuilderAsync(name).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    private async ValueTask<UriBuilder> getEndpointUriBuilderAsync(
        String name,
        String? asset = null) =>
        new(_httpClient.BaseAddress!)
        {
            Path = String.IsNullOrEmpty(asset)
                ? "v2/watchlists:by_name"
                : $"v2/watchlists:by_name/{asset}",
            Query = await new QueryBuilder()
                .AddParameter("name", name.ValidateWatchListName())
                .AsStringAsync().ConfigureAwait(false)
        };

    private static String getEndpointUri(
        Guid watchListId,
        String? asset = null) => 
        String.IsNullOrEmpty(asset)
            ? $"v2/watchlists/{watchListId:D}"
            : $"v2/watchlists/{watchListId:D}/{asset}";
}
