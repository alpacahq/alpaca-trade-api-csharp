using System;
using System.Collections.Generic;
using System.Net.Http;
using static Alpaca.Markets.TimeInterval;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.GetBarsAsync(BarsRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class BarsRequest : Validation.IRequest,
        IRequestWithTimeInterval<IInclusiveTimeInterval>
    {
        /// <summary>
        /// Creates new instance of <see cref="BarsRequest"/> object.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        public BarsRequest(
            String symbol,
            BarTimeFrame timeFrame,
            DateTime from,
            DateTime into)
        {
            Symbol = symbol;
            TimeFrame = timeFrame;
            TimeInterval = GetInclusive(from, into);
        }

        /// <summary>
        /// Gets immutable list of asset names for data retrieval.
        /// </summary>
        public String Symbol { get; }

        /// <summary>
        /// Gets type of time bars for retrieval.
        /// </summary>
        public BarTimeFrame TimeFrame { get; }

        /// <summary>
        /// Gets inclusive date interval for filtering items in response.
        /// </summary>
        public IInclusiveTimeInterval TimeInterval { get; private set; }

        /// <summary>
        /// Gets the pagination parameters for the request (page size and token).
        /// </summary>
        public Pagination Pagination { get; } = new Pagination();

        /// <summary>
        /// Sets the request page size using the fluent interface approach.
        /// </summary>
        /// <param name="pageSize">The request page size.</param>
        /// <returns>The original request parameters object.</returns>
        public BarsRequest WithPageSize(
            UInt32 pageSize)
        {
            Pagination.Size = pageSize;
            return this;
        }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress)
            {
                Path = $"v2/stocks/{Symbol}/bars",
                Query = Pagination.QueryBuilder
                    .AddParameter("start", TimeInterval.From, "O")
                    .AddParameter("end", TimeInterval.Into, "O")
                    .AddParameter("timeframe", TimeFrame)
            };

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (String.IsNullOrEmpty(Symbol))
            {
                yield return new RequestValidationException(
                    "Symbol shouldn't be null or empty.", nameof(Symbol));
            }

            if (Pagination is Validation.IRequest validation)
            {
                foreach (var exception in validation.GetExceptions())
                {
                    yield return exception;
                }
            }
        }

        void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
            IInclusiveTimeInterval value) => TimeInterval = value;
    }
}
