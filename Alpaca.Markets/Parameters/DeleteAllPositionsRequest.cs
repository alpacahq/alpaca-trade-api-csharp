using System;
using System.Net.Http;
using JetBrains.Annotations;

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
        [UsedImplicitly]
        public Boolean? CancelOrders { get; set; }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress!)
            {
                Path = $"v2/positions",
                Query = new QueryBuilder()
                    .AddParameter("cancel_orders", CancelOrders)
            };
    }
}
