using System;
using System.Net.Http;

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

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress)
            {
                Path = "v2/calendar",
                Query = new QueryBuilder()
                    .AddParameter("start", TimeInterval?.From, DateTimeHelper.DateFormat)
                    .AddParameter("end", TimeInterval?.Into, DateTimeHelper.DateFormat)
            };

        void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
            IInclusiveTimeInterval value) => TimeInterval = value;
    }
}
