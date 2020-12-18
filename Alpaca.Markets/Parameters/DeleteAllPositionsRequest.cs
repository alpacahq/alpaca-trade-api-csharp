using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.DeleteAllPositionsAsync(DeleteAllPositionsRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class DeleteAllPositionsRequest
    {
        /// <summary>
        /// Gets the flag indicating that request should also cancel all open orders (false if null).
        /// </summary>
        public Boolean? CancelOrders { get; set; }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress)
            {
                Path = $"v2/positions",
                Query = new QueryBuilder()
                    .AddParameter("cancel_orders", CancelOrders)
            };
    }
}
