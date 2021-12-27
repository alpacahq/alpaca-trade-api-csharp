using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.ListHistoricalQuotesAsync(HistoricalQuotesRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
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

        /// <inheritdoc />
        protected override String LastPathSegment => "quotes";
    }
}
