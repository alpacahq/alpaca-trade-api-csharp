using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
        /// <summary>
        /// Liquidate an open position at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Symbol for liquidation.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>True</c> if position liquidation was accepted.</returns>
        [Obsolete("Use overloaded method that required DeletePositionRequest parameter instead of this one.", true)]
        public async Task<Boolean> DeletePositionAsync(
            String symbol,
            CancellationToken cancellationToken = default) =>
            await _httpClient.DeleteAsync(
                    $"v2/positions/{symbol}", cancellationToken, _alpacaRestApiThrottler)
                .ConfigureAwait(false);

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
        /// Gets list of all trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of trading date information object.</returns>
        [Obsolete("This method will be removed in the next major release.", false)]
        public Task<IReadOnlyList<ICalendar>> ListAllCalendarAsync(
            CancellationToken cancellationToken = default) =>
            ListCalendarAsync(new CalendarRequest(), cancellationToken);

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of order information objects.</returns>
        [Obsolete("This method will be removed in the next major release.", false)]
        public Task<IReadOnlyList<IOrder>> ListAllOrdersAsync(
            CancellationToken cancellationToken = default) =>
            ListOrdersAsync(new ListOrdersRequest(), cancellationToken);
    }
}
