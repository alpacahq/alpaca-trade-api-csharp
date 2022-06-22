using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync(HistoricalQuotesRequest,System.Threading.CancellationToken)"/> call.
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
            : this(new [] { symbol }, from, into)
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
            : this(new [] { symbol }, timeInterval)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public HistoricalQuotesRequest(
            IEnumerable<String> symbols,
            DateTime from,
            DateTime into)
            : base(symbols, from, into)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
        public HistoricalQuotesRequest(
            IEnumerable<String> symbols,
            IInclusiveTimeInterval timeInterval)
            : base(symbols, timeInterval)
        {
        }

        /// <summary>
        /// Gets or sets the feed to pull market data from. The <see cref="MarkedDataFeed.Sip"/> and
        /// <see cref="MarkedDataFeed.Otc"/> are only available to those with a subscription. Default is
        /// <see cref="MarkedDataFeed.Iex"/> for free plans and <see cref="MarkedDataFeed.Sip"/> for paid.
        /// </summary>
        [UsedImplicitly]
        public MarkedDataFeed? Feed { get; set; }

        /// <summary>
        /// Gets or sets the optional parameter for mapping symbol to contract by a specific date.
        /// </summary>
        [UsedImplicitly]
        public DateTime? UseSymbolAsOfTheDate { get; set; }

        /// <inheritdoc />
        protected override String LastPathSegment => "quotes";

        internal override QueryBuilder AddParameters(
            QueryBuilder queryBuilder) => 
            queryBuilder
                .AddParameter("asof", UseSymbolAsOfTheDate, DateTimeHelper.DateFormat)
                .AddParameter("feed", Feed);
    }
}
