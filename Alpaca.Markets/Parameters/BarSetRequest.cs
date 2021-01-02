using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.GetBarSetAsync(BarSetRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class BarSetRequest : Validation.IRequest, 
        IRequestWithTimeInterval<IInclusiveTimeInterval>, IRequestWithTimeInterval<IExclusiveTimeInterval>
    {
        private const Int32 MaxAllowedSymbolsInRequest = 100;

        private readonly List<String> _symbols;

        /// <summary>
        /// Creates new instance of <see cref="BarSetRequest"/> object.
        /// </summary>
        /// <param name="symbol">>Asset name for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        public BarSetRequest(
            String symbol,
            TimeFrame timeFrame)
            : this(new [] { symbol }, timeFrame)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="BarSetRequest"/> object.
        /// </summary>
        /// <param name="symbols">>Asset names for data retrieval.</param>
        /// <param name="timeFrame">Type of time bars for retrieval.</param>
        public BarSetRequest(
            IEnumerable<String> symbols,
            TimeFrame timeFrame)
        {
            _symbols = new List<String>(symbols
                .Distinct(StringComparer.Ordinal));
            TimeFrame = timeFrame;
        }

        /// <summary>
        /// Gets immutable list of asset names for data retrieval.
        /// </summary>
        public IReadOnlyList<String> Symbols => _symbols;

        /// <summary>
        /// Gets type of time bars for retrieval.
        /// </summary>
        public TimeFrame TimeFrame { get; }

        /// <summary>
        /// Gets of sets maximal number of daily bars in data response.
        /// </summary>
        public Int32? Limit { get; set; }

        /// <summary>
        /// Gets flag indicating that <see cref="TimeInterval"/>  are treated as inclusive.
        /// </summary>
        public Boolean AreTimesInclusive => TimeInterval is IInclusiveTimeInterval;

        /// <summary>
        /// Gets inclusive or exclusive date interval for filtering items in response.
        /// </summary>
        public ITimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.InclusiveEmpty;

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress!)
            {
                Path = $"v1/bars/{TimeFrame.ToEnumString()}",
                Query = new QueryBuilder()
                    .AddParameter("symbols", String.Join(",", Symbols))
                    .AddParameter((AreTimesInclusive ? "start" : "after"), TimeInterval.From, "O")
                    .AddParameter((AreTimesInclusive ? "end" : "until"), TimeInterval.Into, "O")
                    .AddParameter("limit", Limit)
            };

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (_symbols.Count == 0)
            {
                yield return new RequestValidationException(
                    "Symbols list shouldn't be empty.", nameof(Symbols));
            }

            if (_symbols.Count > MaxAllowedSymbolsInRequest)
            {
                yield return new RequestValidationException(
                    $"Symbols list shouldn't contain more than {MaxAllowedSymbolsInRequest} items.",
                    nameof(Symbols));
            }

            if (_symbols.Any(String.IsNullOrEmpty))
            {
                yield return new RequestValidationException(
                    "Symbols list shouldn't contain null or empty items.", nameof(Symbols));
            }

            if (TimeFrame == TimeFrame.Hour)
            {
                yield return new RequestValidationException(
                    "1H TimeFrame may not be used for BarSet requests.", nameof(TimeFrame));
            }
        }

        void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
            IInclusiveTimeInterval value) => TimeInterval = value;

        void IRequestWithTimeInterval<IExclusiveTimeInterval>.SetInterval(
            IExclusiveTimeInterval value) => TimeInterval = value;

        internal BarSetRequest SetTimeInterval(
            Boolean areTimesInclusive,
            DateTime? timeFrom,
            DateTime? timeInto) =>
            areTimesInclusive
                ? this.SetTimeInterval(Markets.TimeInterval.GetInclusive(timeFrom, timeInto))
                : this.SetTimeInterval(Markets.TimeInterval.GetExclusive(timeFrom, timeInto));
    }
}
