using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListOrdersAsync(ListOrdersRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class ListOrdersRequest : IRequestWithTimeInterval<IExclusiveTimeInterval>
    {
        private readonly HashSet<String> _symbols = new HashSet<String>(StringComparer.Ordinal);
        
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
        [Obsolete("Use the TimeInterval.From property instead.", true)]
        public DateTime? AfterDateTimeExclusive => TimeInterval?.From;

        /// <summary>
        /// Gets upper bound date time for filtering orders until specified timestamp (exclusive).
        /// </summary>
        [Obsolete("Use the TimeInterval.Into property instead.", true)]
        public DateTime? UntilDateTimeExclusive => TimeInterval?.Into;

        /// <summary>
        /// Sets exclusive time interval for request (start/end time not included into interval if specified).
        /// </summary>
        /// <param name="after">Filtering interval start time.</param>
        /// <param name="until">Filtering interval end time.</param>
        /// <returns>Fluent interface method return same <see cref="ListOrdersRequest"/> instance.</returns>
        [Obsolete("This method will be removed soon in favor of the extension method SetExclusiveTimeInterval.", true)]
        public ListOrdersRequest SetExclusiveTimeIntervalWithNulls(
            DateTime? after,
            DateTime? until) =>
            this.SetTimeInterval(Markets.TimeInterval.GetExclusive(after, until));

        /// <summary>
        /// Gets list of symbols used for filtering the resulting list, if empty - orders for all symbols will be included.
        /// </summary>
        public IEnumerable<String> Symbols => _symbols;

        /// <summary>
        /// Adds a single <paramref name="symbol"/> item into the <see cref="Symbols"/> list.
        /// </summary>
        /// <param name="symbol">Single symbol name for filtering.</param>
        /// <returns>Fluent interface, returns the original <see cref="ListOrdersRequest"/> instance.</returns>
        public ListOrdersRequest WithSymbol(String symbol)
        {
            addSymbolWithCheck(symbol, nameof(symbol));
            return this;
        }

        /// <summary>
        /// Adds all items from the <paramref name="symbols"/> list into the <see cref="Symbols"/> list.
        /// </summary>
        /// <param name="symbols">List of symbol names for filtering.</param>
        /// <returns>Fluent interface, returns the original <see cref="ListOrdersRequest"/> instance.</returns>
        public ListOrdersRequest WithSymbols(IEnumerable<String> symbols)
        {
            foreach (var symbol in symbols.EnsureNotNull(nameof(symbols)))
            {
                addSymbolWithCheck(symbol, nameof(symbols));
            }
            return this;
        }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress)
            {
                Path = "v2/orders",
                Query = new QueryBuilder()
                    .AddParameter("status", OrderStatusFilter)
                    .AddParameter("direction", OrderListSorting)
                    .AddParameter("until", TimeInterval?.Into, "O")
                    .AddParameter("after", TimeInterval?.From, "O")
                    .AddParameter("limit", LimitOrderNumber)
                    .AddParameter("nested", RollUpNestedOrders)
                    .AddParameter("symbols", Symbols.ToArray())
            };

        void IRequestWithTimeInterval<IExclusiveTimeInterval>.SetInterval(
            IExclusiveTimeInterval value) => TimeInterval = value;

        private void addSymbolWithCheck(String symbol, String paramName)
        {
            if (String.IsNullOrEmpty(symbol))
            {
                throw new ArgumentException(
                    "Symbol should be not null nor empty.", paramName);
            }

            _symbols.Add(symbol);
        }
    }
}
