using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListAccountActivitiesAsync(AccountActivitiesRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class AccountActivitiesRequest : IRequestWithTimeInterval<IInclusiveTimeInterval>
    {
        private readonly List<AccountActivityType> _accountActivityTypes = new List<AccountActivityType>();

        /// <summary>
        /// Creates new instance of <see cref="AccountActivitiesRequest"/> object for all activity types.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public AccountActivitiesRequest()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="AccountActivitiesRequest"/> object for a single activity types.
        /// </summary>
        /// <param name="activityType">The activity type you want to view entries for.</param>
        public AccountActivitiesRequest(
            AccountActivityType activityType)
        {
            _accountActivityTypes.Add(activityType);
        }

        /// <summary>
        /// Creates new instance of <see cref="BarSetRequest"/> object for several activity types.
        /// </summary>
        /// <param name="activityTypes">The list of activity types you want to view entries for.</param>
        public AccountActivitiesRequest(
            IEnumerable<AccountActivityType> activityTypes)
        {
            _accountActivityTypes.AddRange(activityTypes.Distinct());
        }

        /// <summary>
        /// Gets the activity types you want to view entries for. Empty list means 'all activity types'.
        /// </summary>
        public IReadOnlyList<AccountActivityType> ActivityTypes => _accountActivityTypes;

        /// <summary>
        /// Gets the date for which you want to see activities.
        /// </summary>
        public DateTime? Date { get; private set; }

        /// <summary>
        /// Gets inclusive date interval for filtering items in response.
        /// </summary>
        public IInclusiveTimeInterval TimeInterval { get; private set; } = Markets.TimeInterval.InclusiveEmpty;

        /// <summary>
        /// Gets or sets the sorting direction for results.
        /// </summary>
        public SortDirection? Direction { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of entries to return in the response.
        /// </summary>
        public Int64? PageSize { get; set; }
        
        /// <summary>
        /// Gets or sets the ID of the end of your current page of results.
        /// </summary>
        public String? PageToken { get; set; }

        /// <summary>
        /// Sets filtering for single <paramref name="date"/> activities.
        /// </summary>
        /// <param name="date">Target date for filtering activities.</param>
        /// <returns>Fluent interface method return same <see cref="AccountActivitiesRequest"/> instance.</returns>
        public AccountActivitiesRequest SetSingleDate(
            DateTime date)
        {
            TimeInterval = Markets.TimeInterval.InclusiveEmpty;
            Date = date;
            return this;
        }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress!)
            {
                Path = "v2/account/activities",
                Query = new QueryBuilder()
                    .AddParameter("activity_types", ActivityTypes)
                    .AddParameter("date", Date, DateTimeHelper.DateFormat)
                    .AddParameter("until", TimeInterval.Into, "O")
                    .AddParameter("after", TimeInterval.From, "O")
                    .AddParameter("direction", Direction)
                    .AddParameter("pageSize", PageSize)
                    .AddParameter("pageToken", PageToken)
            };

        void IRequestWithTimeInterval<IInclusiveTimeInterval>.SetInterval(IInclusiveTimeInterval value)
        {
            TimeInterval = value;
            Date = null;
        }

        internal AccountActivitiesRequest SetTimes(
            DateTime? date = null,
            DateTime? after = null,
            DateTime? until = null)
        {
            if (date is null)
            {
                return this.SetInclusiveTimeInterval(
                    after ?? throw new ArgumentNullException(nameof(after)),
                    until ?? throw new ArgumentNullException(nameof(until)));
            }

            if (until.HasValue || after.HasValue)
            {
                throw new ArgumentException(
                    "You unable to specify 'date' and 'until'/'after' arguments in same call.");
            }

            return SetSingleDate(date.Value);
        }
    }
}
