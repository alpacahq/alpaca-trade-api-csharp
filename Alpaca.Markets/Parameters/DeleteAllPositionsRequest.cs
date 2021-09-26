using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="IAlpacaTradingClient.DeleteAllPositionsAsync(DeleteAllPositionsRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class DeleteAllPositionsRequest
    {
        /// <summary>
        /// Gets or sets the flag indicating that request should also cancel all open orders (false if <c>null</c>).
        /// </summary>
        [UsedImplicitly]
        public Boolean? CancelOrders { get; set; }

        /// <summary>
        /// GEts or sets the operation timeout. Useful in case of deleting lot of positions. The default
        /// HTTP timeout equal to 100 seconds used in case if this property is equal to <c>null</c>.
        /// </summary>
        public TimeSpan? Timeout { get; [UsedImplicitly] set; }

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
