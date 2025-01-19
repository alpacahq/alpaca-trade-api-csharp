namespace Alpaca.Markets;

internal sealed partial class AlpacaTradingClient
{
    public Task<IAccount> GetAccountAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IAccount, JsonAccount>(
            "v2/account", _rateLimitHandler, cancellationToken);

    public Task<IAccountConfiguration> GetAccountConfigurationAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IAccountConfiguration, JsonAccountConfiguration>(
            "v2/account/configurations", _rateLimitHandler, cancellationToken);

    public Task<IAccountConfiguration> PatchAccountConfigurationAsync(
        IAccountConfiguration accountConfiguration,
        CancellationToken cancellationToken = default) =>
        _httpClient.PatchAsync<IAccountConfiguration, JsonAccountConfiguration, IAccountConfiguration>(
            "v2/account/configurations", accountConfiguration.EnsureNotNull(),
            _rateLimitHandler, cancellationToken);

    public async Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
        AccountActivitiesRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IReadOnlyList<IAccountActivity>, List<JsonAccountActivity>>(
            await request.EnsureNotNull()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public async Task<IPortfolioHistory> GetPortfolioHistoryAsync(
        PortfolioHistoryRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IPortfolioHistory, JsonPortfolioHistory>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public async Task<IReadOnlyList<IAsset>> ListAssetsAsync(
        AssetsRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IReadOnlyList<IAsset>, List<JsonAsset>>(
            await request.EnsureNotNull()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IAsset> GetAssetAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IAsset, JsonAsset>(
            $"v2/assets/{symbol}", _rateLimitHandler, cancellationToken);

    public Task<IReadOnlyList<IPosition>> ListPositionsAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IReadOnlyList<IPosition>, List<JsonPosition>>(
            "v2/positions", _rateLimitHandler, cancellationToken);

    public Task<IPosition> GetPositionAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IPosition, JsonPosition>(
            $"v2/positions/{symbol}", _rateLimitHandler, cancellationToken);

    public Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
        CancellationToken cancellationToken = default) =>
        DeleteAllPositionsAsync(new DeleteAllPositionsRequest(), cancellationToken);

    public async Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
        DeleteAllPositionsRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.DeleteAsync<IReadOnlyList<IPositionActionStatus>, List<JsonPositionActionStatus>>(
            await request.EnsureNotNull()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            request.Timeout ?? Timeout.InfiniteTimeSpan,
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public async Task<IOrder> DeletePositionAsync(
        DeletePositionRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.DeleteAsync<IOrder, JsonOrder>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IClock> GetClockAsync(
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IClock, JsonClock>(
            "v2/clock", _rateLimitHandler, cancellationToken);

    public async Task<IReadOnlyList<IIntervalCalendar>> ListIntervalCalendarAsync(
        CalendarRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IReadOnlyList<IIntervalCalendar>, List<JsonCalendar>>(
            await request.EnsureNotNull()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IAnnouncement> GetAnnouncementAsync(
        Guid announcementId,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IAnnouncement, JsonAnnouncement>(
            $"v2/corporate_actions/announcements/{announcementId:D}", _rateLimitHandler, cancellationToken);

    public async Task<IReadOnlyList<IAnnouncement>> ListAnnouncementsAsync(
        AnnouncementsRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IReadOnlyList<IAnnouncement>, List<JsonAnnouncement>>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public async Task<IPage<IOptionContract>> ListOptionContractsAsync(
        OptionContractsRequest request,
        CancellationToken cancellationToken = default) =>
        await _httpClient.GetAsync<IPage<IOptionContract>, JsonOptionContractsPage>(
            await request.EnsureNotNull().Validate()
                .GetUriBuilderAsync(_httpClient).ConfigureAwait(false),
            _rateLimitHandler, cancellationToken).ConfigureAwait(false);

    public Task<IOptionContract> GetOptionContractByIdAsync(
        Guid contractId,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IOptionContract, JsonOptionContract>(
            $"v2/options/contracts/{contractId:D}", _rateLimitHandler, cancellationToken);

    public Task<IOptionContract> GetOptionContractBySymbolAsync(
        String symbol,
        CancellationToken cancellationToken = default) =>
        _httpClient.GetAsync<IOptionContract, JsonOptionContract>(
            $"v2/options/contracts/{symbol.EnsureNotNull()}", _rateLimitHandler, cancellationToken);
}
