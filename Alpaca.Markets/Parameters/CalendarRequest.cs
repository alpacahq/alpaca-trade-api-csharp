using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListCalendarAsync(CalendarRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class CalendarRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
    {
        /// <summary>
        /// Creates new instance of <see cref="CalendarRequest"/> object with the
        /// <see cref="TimeInterval"/> property configured for the single day.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [UsedImplicitly]
        // TODO: olegra - good candidate for the DateOnly type usage
        public static CalendarRequest GetForSingleDay(DateTime date) =>
            new CalendarRequest().SetInclusiveTimeInterval(date.Date, date.Date);

        /// <summary>
        /// Gets inclusive date interval for filtering items in response.
        /// </summary>
        [UsedImplicitly]
        // TODO: olegra - good candidate for the DateOnly type usage
        public IInclusiveTimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.InclusiveEmpty;

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient) =>
            new (httpClient.BaseAddress!)
            {
                Path = "v2/calendar",
                Query = await new QueryBuilder()
                    .AddParameter("start", TimeInterval.From, DateTimeHelper.DateFormat)
                    .AddParameter("end", TimeInterval.Into, DateTimeHelper.DateFormat)
                    .AsStringAsync().ConfigureAwait(false)
            };

        void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
            IInclusiveTimeInterval value) => TimeInterval = value.EnsureNotNull(nameof(value));
    }
}
