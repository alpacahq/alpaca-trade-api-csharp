using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
        /// <summary>
        /// Gets account information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only account information.</returns>
        public Task<IAccount> GetAccountAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IAccount, JsonAccount>(
                "v2/account", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Gets account configuration settings from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of account configuration object.</returns>
        public Task<IAccountConfiguration> GetAccountConfigurationAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IAccountConfiguration, JsonAccountConfiguration>(
                "v2/account/configurations", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Updates account configuration settings using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="accountConfiguration">New account configuration object for updating.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of updated account configuration object.</returns>
        public Task<IAccountConfiguration> PatchAccountConfigurationAsync(
            IAccountConfiguration accountConfiguration,
            CancellationToken cancellationToken = default) =>
            _httpClient.PatchAsync<IAccountConfiguration, JsonAccountConfiguration, IAccountConfiguration>(
                "v2/account/configurations", accountConfiguration,
                _alpacaRestApiThrottler, cancellationToken);

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint by specific activity.
        /// </summary>
        /// <param name="request">Account activities request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of account activity record objects.</returns>
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            AccountActivitiesRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IAccountActivity>, List<JsonAccountActivity>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Gets portfolio equity history from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Portfolio history request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only portfolio history information object.</returns>
        public Task<IPortfolioHistory> GetPortfolioHistoryAsync(
            PortfolioHistoryRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPortfolioHistory, JsonPortfolioHistory>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Gets list of all available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        [Obsolete("This method will be removed in the next major release.", false)]
        public Task<IReadOnlyList<IAsset>> ListAllAssetsAsync(
            CancellationToken cancellationToken = default) =>
            ListAssetsAsync(new AssetsRequest(), cancellationToken);

        /// <summary>
        /// Gets list of available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Asset list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        public Task<IReadOnlyList<IAsset>> ListAssetsAsync(
            AssetsRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IAsset>, List<JsonAsset>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Get single asset information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only asset information.</returns>
        public Task<IAsset> GetAssetAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IAsset, JsonAsset>(
                $"v2/assets/{symbol}", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Gets list of available positions from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of position information objects.</returns>
        public Task<IReadOnlyList<IPosition>> ListPositionsAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IPosition>, List<JsonPosition>>(
                "v2/positions", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Gets position information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Position asset name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only position information object.</returns>
        public Task<IPosition> GetPositionAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IPosition, JsonPosition>(
                $"v2/positions/{symbol}", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Liquidates all open positions at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of position cancellation status objects.</returns>
        public Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync<IReadOnlyList<IPositionActionStatus>, List<JsonPositionActionStatus>>(
                    "v2/positions", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Liquidate an open position at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Position deletion request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The <see cref="IOrder"/> object that represents the position liquidation order (for tracking).</returns>
        public Task<IOrder> DeletePositionAsync(
            DeletePositionRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync<IOrder, JsonOrder>(
                request.EnsureNotNull(nameof(request)).Validate().GetUriBuilder(_httpClient), 
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Get current time information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only clock information object.</returns>
        public Task<IClock> GetClockAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IClock, JsonClock>(
                "v2/clock", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Gets list of all trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of trading date information object.</returns>
        [Obsolete("This method will be removed in the next major release.", false)]
        public Task<IReadOnlyList<ICalendar>> ListAllCalendarAsync(
            CancellationToken cancellationToken = default) =>
            ListCalendarAsync(new CalendarRequest(), cancellationToken);

        /// <summary>
        /// Gets list of trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Calendar items request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of trading date information object.</returns>
        public Task<IReadOnlyList<ICalendar>> ListCalendarAsync(
            CalendarRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<ICalendar>, List<JsonCalendar>>(
                request.EnsureNotNull(nameof(request)).GetUriBuilder(_httpClient),
                cancellationToken, _alpacaRestApiThrottler);
    }
}
