using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base logic for all historical data requests on Alpaca Data API v2.
    /// </summary>
    public abstract class HistoricalRequestBase : Validation.IRequest
    {
        /// <summary>
        /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
        /// </summary>
        /// <param name="symbol">Asset name for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        protected HistoricalRequestBase(
            String symbol,
            DateTime from,
            DateTime into)
        {
            Symbol = symbol ?? throw new ArgumentException(
                "Symbol name cannot be null.", nameof(symbol));
            TimeInterval = Markets.TimeInterval.GetInclusive(from, into);
        }

        /// <summary>
        /// Gets asset name for data retrieval.
        /// </summary>
        public String Symbol { get; }

        /// <summary>
        /// Gets inclusive date interval for filtering items in response.
        /// </summary>
        [UsedImplicitly]
        public IInclusiveTimeInterval TimeInterval { get; }

        /// <summary>
        /// Gets the pagination parameters for the request (page size and token).
        /// </summary>
        public Pagination Pagination { get; } = new ();

        /// <summary>
        /// Gets the last part of the full REST endpoint URL path.
        /// </summary>
        protected abstract String LastPathSegment { get; }

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient) =>
            new (httpClient.BaseAddress!)
            {
                Path = $"v2/stocks/{Symbol}/{LastPathSegment}",
                Query = await AddParameters(Pagination.QueryBuilder
                    .AddParameter("start", TimeInterval.From, "O")
                    .AddParameter("end", TimeInterval.Into, "O"))
                    .AsStringAsync().ConfigureAwait(false)
            };

        internal virtual QueryBuilder AddParameters(
            QueryBuilder queryBuilder) => queryBuilder;

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (String.IsNullOrEmpty(Symbol))
            {
                yield return new RequestValidationException(
                    "Symbol shouldn't be empty.", nameof(Symbol));
            }

            if (Pagination is Validation.IRequest validation)
            {
                foreach (var exception in validation.GetExceptions())
                {
                    yield return exception;
                }
            }
        }
    }
}
