using System;
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
                    _alpacaRestApiThrottler, $"positions/{symbol}", cancellationToken)
                .ConfigureAwait(false);
    }
}
