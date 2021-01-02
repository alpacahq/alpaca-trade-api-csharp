using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.GetPortfolioHistoryAsync(PortfolioHistoryRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    [UsedImplicitly]
    public sealed class PortfolioHistoryRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
    {
        /// <summary>
        /// Gets inclusive date interval for filtering items in response.
        /// </summary>
        [UsedImplicitly]
        public IInclusiveTimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.InclusiveEmpty;

        /// <summary>
        /// Gets or sets the time frame value for desired history. Default value (if <c>null</c>) is 1 minute
        /// for a period shorter than 7 days, 15 minutes for a period less than 30 days, or 1 day for a longer period.
        /// </summary>
        [UsedImplicitly]
        public TimeFrame? TimeFrame { get; set; }

        /// <summary>
        /// Gets or sets period value for desired history. Default value (if <c>null</c>) is 1 month.
        /// </summary>
        [UsedImplicitly]
        public HistoryPeriod? Period { get; set; }

        /// <summary>
        /// Gets or sets flags, indicating that include extended hours included in the result.
        /// This is effective only for time frame less than 1 day.
        /// </summary>
        [UsedImplicitly]
        public Boolean? ExtendedHours { get; set; }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress!)
            {
                Path = "v2/account/portfolio/history",
                Query = new QueryBuilder()
                    .AddParameter("start_date", TimeInterval.From, DateTimeHelper.DateFormat)
                    .AddParameter("end_date", TimeInterval.Into, DateTimeHelper.DateFormat)
                    .AddParameter("period", Period?.ToString())
                    .AddParameter("timeframe", TimeFrame)
                    .AddParameter("extended_hours", ExtendedHours)
            };

        void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(
            IInclusiveTimeInterval value) => TimeInterval = value.EnsureNotNull(nameof(value));
    }
}
