using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

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

        /// <summary>
        /// Gets or sets the feed to pull market data from. The <see cref="MarkedDataFeed.Sip"/> and
        /// <see cref="MarkedDataFeed.Otc"/> are only available to those with a subscription. Default is
        /// <see cref="MarkedDataFeed.Iex"/> for free plans and <see cref="MarkedDataFeed.Sip"/> for paid.
        /// </summary>
        [UsedImplicitly]
        public MarkedDataFeed? Feed { get; set; }

        /// <inheritdoc />
        protected override String LastPathSegment => "quotes";

        internal override QueryBuilder AddParameters(
            QueryBuilder queryBuilder) => 
            queryBuilder.AddParameter("feed", Feed);
    }
}
