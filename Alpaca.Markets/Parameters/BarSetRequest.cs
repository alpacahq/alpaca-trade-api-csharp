using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaDataClient.GetBarSetAsync(BarSetRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class BarSetRequest
    {
        internal const Int32 DefaultLimit = 100;

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
            _symbols = new List<String>(symbols);
            TimeFrame = timeFrame;
        }

        /// <summary>
        /// Gets mutable list of asset names for data retrieval.
        /// </summary>
        public IList<String> Symbols => _symbols;

        /// <summary>
        /// Gets or sets type of time bars for retrieval.
        /// </summary>
        public TimeFrame TimeFrame { get; set; }

        /// <summary>
        /// Gets of sets maximal number of daily bars in data response.
        /// </summary>
        public Int32? Limit { get; set; } = DefaultLimit;

        /// <summary>
        /// If <c>true</c> - both <see cref="TimeFrom"/> and <see cref="TimeInto"/> properties are treated as inclusive.
        /// </summary>
        public Boolean AreTimesInclusive { get; set; } = true;

        /// <summary>
        /// Gets or sets start time for filtering.
        /// </summary>
        public DateTime? TimeFrom { get; set; }

        /// <summary>
        /// Gets or sets end time for filtering.
        /// </summary>
        public DateTime? TimeInto { get; set; }

        /// <summary>
        /// Validates parameters consistency and throws exception in case of any errors.
        /// </summary>
        /// <exception cref="RequestValidationException"></exception>
        public void Validate()
        {
            if (_symbols.Count == 0)
            {
                throw new RequestValidationException(
                    "Symbols list shouldn't be empty.", nameof(Symbols));
            }

            if (TimeFrom > TimeInto)
            {
                throw new RequestValidationException(
                    "Time interval should be valid.");
            }

            if (TimeFrame == TimeFrame.Hour)
            {
                throw new RequestValidationException(
                    "1H TimeFrame may not be used for BarSet requests.");
            }
        }
    }
}