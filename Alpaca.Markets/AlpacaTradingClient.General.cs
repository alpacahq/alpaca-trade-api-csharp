using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
        /// <inheritdoc />
        public Task<IAccount> GetAccountAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IAccount, JsonAccount>(
                "v2/account", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IAccountConfiguration> GetAccountConfigurationAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IAccountConfiguration, JsonAccountConfiguration>(
                "v2/account/configurations", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IAccountConfiguration> PatchAccountConfigurationAsync(
            IAccountConfiguration accountConfiguration,
            CancellationToken cancellationToken = default) =>
            _httpClient.PatchAsync<IAccountConfiguration, JsonAccountConfiguration, IAccountConfiguration>(
                "v2/account/configurations", accountConfiguration,
                _alpacaRestApiThrottler, cancellationToken);

        /// <inheritdoc />
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            AccountActivitiesRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IAccountActivity>, List<JsonAccountActivity>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IPortfolioHistory> GetPortfolioHistoryAsync(
            PortfolioHistoryRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPortfolioHistory, JsonPortfolioHistory>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IReadOnlyList<IAsset>> ListAssetsAsync(
            AssetsRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IAsset>, List<JsonAsset>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IAsset> GetAssetAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IAsset, JsonAsset>(
                $"v2/assets/{symbol}", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IReadOnlyList<IPosition>> ListPositionsAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IPosition>, List<JsonPosition>>(
                "v2/positions", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IPosition> GetPositionAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPosition, JsonPosition>(
                $"v2/positions/{symbol}", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
            CancellationToken cancellationToken = default) =>
            DeleteAllPositionsAsync(new DeleteAllPositionsRequest(), cancellationToken);

        /// <inheritdoc />
        public Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
            DeleteAllPositionsRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync<IReadOnlyList<IPositionActionStatus>, List<JsonPositionActionStatus>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient), 
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IOrder> DeletePositionAsync(
            DeletePositionRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync<IOrder, JsonOrder>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient), 
                cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IClock> GetClockAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IClock, JsonClock>(
                "v2/clock", cancellationToken, _alpacaRestApiThrottler);

        /// <inheritdoc />
        public Task<IReadOnlyList<ICalendar>> ListCalendarAsync(
            CalendarRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<ICalendar>, List<JsonCalendar>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);
    }
}
