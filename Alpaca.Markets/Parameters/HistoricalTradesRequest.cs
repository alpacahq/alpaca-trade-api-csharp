using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for
    /// <see cref="IAlpacaDataClient.ListHistoricalTradesAsync(HistoricalTradesRequest,System.Threading.CancellationToken)"/> and
    /// <see cref="IAlpacaDataClient.GetHistoricalTradesAsync(HistoricalTradesRequest,System.Threading.CancellationToken)"/> calls.
    /// </summary>
    [UsedImplicitly]
    public sealed class HistoricalTradesRequest : HistoricalRequestBase
    {
        /// <summary>
        /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public HistoricalTradesRequest(
            String symbol,
            DateTime from,
            DateTime into)
            : this(new [] { symbol }, from, into)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
        public HistoricalTradesRequest(
            String symbol,
            IInclusiveTimeInterval timeInterval)
            : this(new [] { symbol }, timeInterval)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public HistoricalTradesRequest(
            IEnumerable<String> symbols,
            DateTime from,
            DateTime into)
            : base(symbols, from, into)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalTradesRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
        public HistoricalTradesRequest(
            IEnumerable<String> symbols,
            IInclusiveTimeInterval timeInterval)
            : base(symbols, timeInterval)
        {
        }

        /// <inheritdoc />
        protected override String LastPathSegment => "trades";
    }
}
