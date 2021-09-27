using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for
    /// <see cref="IAlpacaCryptoDataClient.ListHistoricalQuotesAsync(HistoricalCryptoQuotesRequest,System.Threading.CancellationToken)"/> and
    /// <see cref="IAlpacaCryptoDataClient.GetHistoricalQuotesAsync(HistoricalCryptoQuotesRequest,System.Threading.CancellationToken)"/> calls.
    /// </summary>
    [UsedImplicitly]
    public sealed class HistoricalCryptoQuotesRequest : HistoricalCryptoRequestBase
    {
        /// <summary>
        /// Creates new instance of <see cref="HistoricalCryptoQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public HistoricalCryptoQuotesRequest(
            String symbol,
            DateTime from,
            DateTime into)
            : this(new [] { symbol }, from, into)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalCryptoQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
        public HistoricalCryptoQuotesRequest(
            String symbol,
            IInclusiveTimeInterval timeInterval)
            : this(new [] { symbol }, timeInterval)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalCryptoQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public HistoricalCryptoQuotesRequest(
            IEnumerable<String> symbols,
            DateTime from,
            DateTime into)
            : base(symbols, from, into)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalCryptoQuotesRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
        public HistoricalCryptoQuotesRequest(
            IEnumerable<String> symbols,
            IInclusiveTimeInterval timeInterval)
            : base(symbols, timeInterval)
        {
        }

        private HistoricalCryptoQuotesRequest(
            HistoricalCryptoQuotesRequest request,
            IEnumerable<CryptoExchange> exchanges)
            : base(request.Symbols, request.TimeInterval,
                request.Exchanges.Concat(exchanges))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalCryptoQuotesRequest"/> object
        /// with the updated <see cref="HistoricalCryptoRequestBase.Exchanges"/> list.
        /// </summary>
        /// <param name="exchanges">Crypto exchanges to add into the list.</param>
        /// <returns>The new instance of the <see cref="HistoricalCryptoQuotesRequest"/> object.</returns>
        [UsedImplicitly]
        public HistoricalCryptoQuotesRequest WithExchanges(
            IEnumerable<CryptoExchange> exchanges) =>
            new (this, exchanges);

        /// <summary>
        /// Creates new instance of <see cref="HistoricalCryptoQuotesRequest"/> object
        /// with the updated <see cref="HistoricalCryptoRequestBase.Exchanges"/> list.
        /// </summary>
        /// <param name="exchanges">Crypto exchanges to add into the list.</param>
        /// <returns>The new instance of the <see cref="HistoricalCryptoQuotesRequest"/> object.</returns>
        [UsedImplicitly]
        public HistoricalCryptoQuotesRequest WithExchanges(
            params CryptoExchange[] exchanges) =>
            new (this, exchanges);

        /// <inheritdoc />
        protected override String LastPathSegment => "quotes";
    }
}
