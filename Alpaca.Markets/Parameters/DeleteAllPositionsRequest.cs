using System;
using System.Net.Http;
using System.Threading.Tasks;
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

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient) =>
            new (httpClient.BaseAddress!)
            {
                Path = "v2/positions",
                Query = await new QueryBuilder()
                    .AddParameter("cancel_orders", CancelOrders)
                    .AsStringAsync().ConfigureAwait(false)
            };
    }
}
