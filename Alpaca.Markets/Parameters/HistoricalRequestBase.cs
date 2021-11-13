using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly HashSet<String> _symbols = new (StringComparer.Ordinal);

        /// <summary>
        /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        protected internal HistoricalRequestBase(
            IEnumerable<String> symbols,
            DateTime from,
            DateTime into)
            : this(symbols, Markets.TimeInterval.GetInclusive(from, into))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="HistoricalRequestBase"/> object.
        /// </summary>
        /// <param name="symbols">Asset names for data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for filtering items in response.</param>
        protected internal HistoricalRequestBase(
            IEnumerable<String> symbols,
            IInclusiveTimeInterval timeInterval)
        {
            _symbols.UnionWith(symbols.EnsureNotNull(nameof(symbols)));
            TimeInterval = timeInterval.EnsureNotNull(nameof(timeInterval));
        }

        /// <summary>
        /// Gets asset name for data retrieval.
        /// </summary>
        [UsedImplicitly]
        [Obsolete("This property is obsolete and will be removed in the next major version of SDK. Use the `Symbols` property instead of this one.", false)]
        public String Symbol => _symbols.FirstOrDefault() ?? String.Empty;

        /// <summary>
        /// Gets assets names list for data retrieval.
        /// </summary>
        public IReadOnlyCollection<String> Symbols => _symbols;

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
            new UriBuilder(httpClient.BaseAddress!)
            {
                Query = await AddParameters(Pagination.QueryBuilder
                        .AddParameter("symbols",
                            HasSingleSymbol ? Array.Empty<String>() : Symbols)
                        .AddParameter("start", TimeInterval.From, "O")
                        .AddParameter("end", TimeInterval.Into, "O"))
                    .AsStringAsync().ConfigureAwait(false)
            }.AppendPath(HasSingleSymbol
                ? $"{Symbols.First()}/{LastPathSegment}"
                : $"{LastPathSegment}");

        internal virtual QueryBuilder AddParameters(
            QueryBuilder queryBuilder) => queryBuilder;

        internal bool HasSingleSymbol => Symbols.Count == 1;

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (_symbols.Count == 0)
            {
                yield return new RequestValidationException(
                    "Symbols list shouldn't be empty.", nameof(Symbols));
            }

            if (Symbols.Any(String.IsNullOrEmpty))
            {
                yield return new RequestValidationException(
                    "Symbol shouldn't be empty.", nameof(Symbols));
            }

            if (Pagination is not Validation.IRequest validation)
            {
                yield break;
            }

            foreach (var exception in validation.GetExceptions())
            {
                yield return exception;
            }
        }
    }
}
