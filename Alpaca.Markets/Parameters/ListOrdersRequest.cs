using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListOrdersAsync(ListOrdersRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public class ListOrdersRequest : Validation.IRequest
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
        /// Gets upper bound date time for filtering orders until specified timestamp (exclusive).
        /// </summary>
        public DateTime? UntilDateTimeExclusive { get; private set; }
        
        /// <summary>
        /// Gets lower bound date time for filtering orders until specified timestamp (exclusive).
        /// </summary>
        public DateTime? AfterDateTimeExclusive { get; private set; }
        
        /// <summary>
        /// Gets or sets maximal number of orders in response.
        /// </summary>
        public Int64? LimitOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets flag for rolling up multi-leg orders under the <see cref="IOrder.Legs"/> property of primary order.
        /// </summary>
        public Boolean? RollUpNestedOrders { get; set; }

        /// <summary>
        /// Sets exclusive time interval for request (start/end time not included into interval if specified).
        /// </summary>
        /// <param name="after">Filtering interval start time.</param>
        /// <param name="until">Filtering interval end time.</param>
        /// <returns>Fluent interface method return same <see cref="ListOrdersRequest"/> instance.</returns>
        public ListOrdersRequest SetExclusiveTimeInterval(
            DateTime? after,
            DateTime? until)
        {
            AfterDateTimeExclusive = after;
            UntilDateTimeExclusive = until;
            return this;
        }

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (AfterDateTimeExclusive > UntilDateTimeExclusive)
            {
                yield return new RequestValidationException(
                    "Time interval should be valid.", nameof(UntilDateTimeExclusive));
                yield return new RequestValidationException(
                    "Time interval should be valid.", nameof(AfterDateTimeExclusive));
            }
        }
    }
}
