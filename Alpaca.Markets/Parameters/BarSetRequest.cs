using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.GetBarSetAsync(BarSetRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class BarSetRequest : Validation.IRequest
    {
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
                .Where(_ => !String.IsNullOrEmpty(_))
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
        /// Gets flag indicating that both <see cref="TimeFrom"/> and <see cref="TimeInto"/> properties are treated as inclusive timestamps.
        /// </summary>
        public Boolean AreTimesInclusive { get; private set; } = true;

        /// <summary>
        /// Gets start time for filtering.
        /// </summary>
        public DateTime? TimeFrom { get; private set; }

        /// <summary>
        /// Gets end time for filtering.
        /// </summary>
        public DateTime? TimeInto { get; private set; }

        /// <summary>
        /// Sets inclusive time interval for request (start/end time included into interval if specified).
        /// </summary>
        /// <param name="start">Filtering interval start time.</param>
        /// <param name="end">Filtering interval end time.</param>
        /// <returns>Fluent interface method return same <see cref="BarSetRequest"/> instance.</returns>
        public BarSetRequest SetInclusiveTimeInterval(
            DateTime? start,
            DateTime? end) =>
            SetTimeInterval(
                true, start, end);

        /// <summary>
        /// Sets exclusive time interval for request (start/end time not included into interval if specified).
        /// </summary>
        /// <param name="after">Filtering interval start time.</param>
        /// <param name="until">Filtering interval end time.</param>
        /// <returns>Fluent interface method return same <see cref="BarSetRequest"/> instance.</returns>
        public BarSetRequest SetExclusiveTimeInterval(
            DateTime? after,
            DateTime? until) =>
            SetTimeInterval(
                false, after, until);

        /// <summary>
        /// Gets all validation exceptions (inconsistent request data errors).
        /// </summary>
        /// <returns>Lazy-evaluated list of validation errors.</returns>
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (_symbols.Count == 0)
            {
                yield return new RequestValidationException(
                    "Symbols list shouldn't be empty.", nameof(Symbols));
            }

            if (TimeFrom > TimeInto)
            {
                yield return new RequestValidationException(
                    "Time interval should be valid.", nameof(TimeFrom));
                yield return new RequestValidationException(
                    "Time interval should be valid.", nameof(TimeInto));
            }

            if (TimeFrame == TimeFrame.Hour)
            {
                yield return new RequestValidationException(
                    "1H TimeFrame may not be used for BarSet requests.", nameof(TimeFrame));
            }
        }

        internal BarSetRequest SetTimeInterval(
            Boolean areTimesInclusive,
            DateTime? timeFrom,
            DateTime? timeInto)
        {
            AreTimesInclusive = areTimesInclusive;
            TimeFrom = timeFrom;
            TimeInto = timeInto;
            return this;
        }
    }
}
