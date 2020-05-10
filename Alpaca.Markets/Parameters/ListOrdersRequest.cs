using System;

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
        public OrderStatusFilter? OrderStatusFilter { get; set; }

        /// <summary>
        /// Gets or sets the chronological order of response based on the submission time.
        /// </summary>
        public SortDirection? OrderListSorting { get; set; }

        /// <summary>
        /// Gets exclusive date time interval for filtering orders in response.
        /// </summary>
        public IExclusiveTimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.ExclusiveEmpty;

        /// <summary>
        /// Gets or sets maximal number of orders in response.
        /// </summary>
        public Int64? LimitOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets flag for rolling up multi-leg orders under the <see cref="IOrder.Legs"/> property of primary order.
        /// </summary>
        public Boolean? RollUpNestedOrders { get; set; }

        /// <summary>
        /// Gets lower bound date time for filtering orders until specified timestamp (exclusive).
        /// </summary>
        [Obsolete("Use the TimeInterval.From property instead.", false)]
        public DateTime? AfterDateTimeExclusive => TimeInterval?.From;

        /// <summary>
        /// Gets upper bound date time for filtering orders until specified timestamp (exclusive).
        /// </summary>
        [Obsolete("Use the TimeInterval.Into property instead.", false)]
        public DateTime? UntilDateTimeExclusive => TimeInterval?.Into;

        /// <summary>
        /// Sets exclusive time interval for request (start/end time not included into interval if specified).
        /// </summary>
        /// <param name="after">Filtering interval start time.</param>
        /// <param name="until">Filtering interval end time.</param>
        /// <returns>Fluent interface method return same <see cref="ListOrdersRequest"/> instance.</returns>
        [Obsolete("This method will be removed soon in favor of the extension method SetExclusiveTimeInterval.", false)]
        public ListOrdersRequest SetExclusiveTimeIntervalWithNulls(
            DateTime? after,
            DateTime? until) =>
            this.SetTimeInterval(Markets.TimeInterval.GetExclusive(after, until));

        void IRequestWithTimeInterval<IExclusiveTimeInterval>.SetInterval(
            IExclusiveTimeInterval value) => TimeInterval = value;
    }
}
