using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
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
            : base(symbol, from, into)
        {
        }

        /// <inheritdoc />
        protected override String UrlPath => "trades";
    }
}
