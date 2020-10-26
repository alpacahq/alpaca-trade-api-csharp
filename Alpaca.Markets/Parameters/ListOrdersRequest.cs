using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListOrdersAsync(ListOrdersRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class ListOrdersRequest : IRequestWithTimeInterval<IExclusiveTimeInterval>
    {
        /// <summary>
        /// Gets or sets order status for filtering.
        /// </summary>
        [UsedImplicitly] 
        public OrderStatusFilter? OrderStatusFilter { get; set; }

        /// <summary>
        /// Gets or sets the chronological order of response based on the submission time.
        /// </summary>
        [UsedImplicitly] 
        public SortDirection? OrderListSorting { get; set; }

        /// <summary>
        /// Gets exclusive date time interval for filtering orders in response.
        /// </summary>
        [UsedImplicitly] 
        public IExclusiveTimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.ExclusiveEmpty;

        /// <summary>
        /// Gets or sets maximal number of orders in response.
        /// </summary>
        [UsedImplicitly] 
        public Int64? LimitOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets flag for rolling up multi-leg orders under the <see cref="IOrder.Legs"/> property of primary order.
        /// </summary>
        [UsedImplicitly] 
        public Boolean? RollUpNestedOrders { get; set; }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress)
            {
                Path = "v2/orders",
                Query = new QueryBuilder()
                    .AddParameter("status", OrderStatusFilter)
                    .AddParameter("direction", OrderListSorting)
                    .AddParameter("until", TimeInterval.Into, "O")
                    .AddParameter("after", TimeInterval.From, "O")
                    .AddParameter("limit", LimitOrderNumber)
                    .AddParameter("nested", RollUpNestedOrders)
            };

        void IRequestWithTimeInterval<IExclusiveTimeInterval>.SetInterval(
            IExclusiveTimeInterval value) => TimeInterval = value.EnsureNotNull(nameof(value));
    }
}
