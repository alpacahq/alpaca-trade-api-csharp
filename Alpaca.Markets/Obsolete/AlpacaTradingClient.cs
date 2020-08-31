using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
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
