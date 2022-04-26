using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.ListHistoricalBarsAsync(HistoricalBarsRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class HistoricalBarsRequest : HistoricalRequestBase
    {
        /// <summary>
        /// Creates new instance of <see cref="HistoricalBarsRequest"/> object.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public HistoricalBarsRequest(
            String symbol,
            DateTime from,
            DateTime into,
            BarTimeFrame timeFrame)
            : base(symbol, from, into) =>
            TimeFrame = timeFrame;

        /// <summary>
        /// Gets type of time bars for retrieval.
        /// </summary>
        [UsedImplicitly]
        public BarTimeFrame TimeFrame { get; }

        /// <summary>
        /// Gets or sets adjustment type of time bars for retrieval.
        /// </summary>
        [UsedImplicitly]
        public Adjustment? Adjustment { get; set; }

        /// <summary>
        /// Gets or sets the feed to pull market data from. The <see cref="MarkedDataFeed.Sip"/> and
        /// <see cref="MarkedDataFeed.Otc"/> are only available to those with a subscription. Default is
        /// <see cref="MarkedDataFeed.Iex"/> for free plans and <see cref="MarkedDataFeed.Sip"/> for paid.
        /// </summary>
        [UsedImplicitly]
        public MarkedDataFeed? Feed { get; set; }

        /// <inheritdoc />
        protected override String LastPathSegment => "bars";

        internal override QueryBuilder AddParameters(
            QueryBuilder queryBuilder) => 
            queryBuilder
                // ReSharper disable once StringLiteralTypo
                .AddParameter("timeframe", TimeFrame.ToString())
                .AddParameter("adjustment", Adjustment)
                .AddParameter("feed", Feed);
    }
}
