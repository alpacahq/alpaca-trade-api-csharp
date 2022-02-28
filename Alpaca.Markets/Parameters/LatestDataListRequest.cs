using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates data for latest crypto data requests on Alpaca Data API v2.
    /// </summary>
    public sealed class LatestDataListRequest : Validation.IRequest
    {
        private readonly HashSet<String> _symbols = new(StringComparer.Ordinal);

        /// <summary>
        /// Creates new instance of <see cref="LatestDataListRequest"/> object.
        /// </summary>
        /// <param name="symbols">Asset name for data retrieval.</param>
        /// <param name="exchange">Crypto exchange for data retrieval.</param>
        public LatestDataListRequest(
            IEnumerable<String> symbols,
            CryptoExchange exchange)
        {
            _symbols.UnionWith(symbols.EnsureNotNull(nameof(symbols)));
            Exchange = exchange;
        }

        /// <summary>
        /// Gets asset name for data retrieval.
        /// </summary>
        [UsedImplicitly]
        public IReadOnlyCollection<String> Symbols => _symbols;

        /// <summary>
        /// Gets crypto exchange for data retrieval.
        /// </summary>
        [UsedImplicitly]
        public CryptoExchange Exchange { get; }

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient,
            String lastPathSegment) =>
            new UriBuilder(httpClient.BaseAddress!)
            {
                Query = await new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", _symbols))
                    .AddParameter("exchange", Exchange.ToEnumString())
                    .AsStringAsync().ConfigureAwait(false)
            }.AppendPath($"{lastPathSegment}/latest");

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
        }
    }
}
