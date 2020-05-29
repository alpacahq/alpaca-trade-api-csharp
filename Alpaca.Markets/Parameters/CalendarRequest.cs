using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListCalendarAsync(CalendarRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class CalendarRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
    {
        /// <summary>
        /// Gets inclusive date interval for filtering items in response.
        /// </summary>
        public IInclusiveTimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.InclusiveEmpty;

        /// <summary>
        /// Gets start time for filtering (inclusive).
        /// </summary>
        [Obsolete("Use the TimeInterval.From property instead.", true)]
        public DateTime? StartDateInclusive => TimeInterval?.From;

        /// <summary>
        /// Gets end time for filtering (inclusive).
        /// </summary>
        [Obsolete("Use the TimeInterval.Into property instead.", true)]
        public DateTime? EndDateInclusive => TimeInterval?.Into;

        /// <summary>
        /// Sets exclusive time interval for request (start/end time included into interval if specified).
        /// </summary>
        /// <param name="start">Filtering interval start time.</param>
        /// <param name="end">Filtering interval end time.</param>
        /// <returns>Fluent interface method return same <see cref="CalendarRequest"/> instance.</returns>
        [Obsolete("This method will be removed soon in favor of the extension method SetInclusiveTimeInterval.", true)]
        public CalendarRequest SetInclusiveTimeIntervalWithNulls(
            DateTime? start,
            DateTime? end) =>
            this.SetTimeInterval(Markets.TimeInterval.GetInclusive(start, end));

        void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
            IInclusiveTimeInterval value) => TimeInterval = value;
    }
}
