using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.ListHistoricalQuotesAsync(HistoricalQuotesRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    [UsedImplicitly]
    public sealed class HistoricalQuotesRequest : HistoricalRequestBase
    {
        /// <summary>
        /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public HistoricalQuotesRequest(
            String symbol,
            DateTime from,
            DateTime into)
            : base(symbol, from, into)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
        public HistoricalQuotesRequest(
            String symbol,
            IInclusiveTimeInterval timeInterval)
            : base(symbol, timeInterval)
        {
        }

        /// <inheritdoc />
        protected override String LastPathSegment => "quotes";
    }
}
