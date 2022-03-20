using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListAssetsAsync(AssetsRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class AnnouncementsRequest : Validation.IRequest
    {
        private readonly HashSet<CorporateActionType> _corporateActionTypes = new ();

        /// <summary>
        /// Creates new instance of <see cref="AnnouncementsRequest"/> object.
        /// </summary>
        /// <param name="corporateActionType">Single corporate action type for filtering.</param>
        /// <param name="timeInterval">Date range when searching corporate action announcements.</param>
        public AnnouncementsRequest(
            CorporateActionType corporateActionType,
            IInclusiveTimeInterval timeInterval)
        {
            _corporateActionTypes.Add(corporateActionType);
            TimeInterval = timeInterval;
        }

        /// <summary>
        /// Creates new instance of <see cref="AnnouncementsRequest"/> object.
        /// </summary>
        /// <param name="corporateActionTypes">List of the corporate action types for filtering.</param>
        /// <param name="timeInterval">Date range when searching corporate action announcements.</param>
        public AnnouncementsRequest(
            IEnumerable<CorporateActionType> corporateActionTypes,
            IInclusiveTimeInterval timeInterval)
        {
            _corporateActionTypes.UnionWith(corporateActionTypes);
            TimeInterval = timeInterval;
        }

        /// <summary>
        /// Gets the list of the corporate action types for filtering.
        /// </summary>
        public IReadOnlyCollection<CorporateActionType> CorporateActionTypes => _corporateActionTypes;

        /// <summary>
        /// Gets the date range when searching corporate action announcements.
        /// </summary>
        [UsedImplicitly]
        public IInclusiveTimeInterval TimeInterval { get; private set; }

        /// <summary>
        /// Gets or sets the type of date for filtering by <see cref="TimeInterval"/> parameter.
        /// </summary>
        [UsedImplicitly]
        public CorporateActionDateType? DateType { get; set; }

        /// <summary>
        /// Gets or sets the symbol of the company initiating the announcement.
        /// </summary>
        [UsedImplicitly]
        public String? Symbol { get; set; }

        /// <summary>
        /// Gets or sets the CUSIP of the company initiating the announcement.
        /// </summary>
        [UsedImplicitly]
        public String? Cusip { get; set; }

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient) =>
            new (httpClient.BaseAddress!)
            {
                Path = "v2/corporate_actions/announcements",
                Query = await new QueryBuilder()
                    .AddParameter("ca_types", CorporateActionTypes)
                    .AddParameter("since", TimeInterval.From, DateTimeHelper.DateFormat)
                    .AddParameter("until", TimeInterval.Into, DateTimeHelper.DateFormat)
                    .AddParameter("date_type", DateType)
                    .AddParameter("symbol", Symbol)
                    .AddParameter("cusip", Cusip)
                    .AsStringAsync().ConfigureAwait(false)
            };

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (_corporateActionTypes.Count == 0)
            {
                yield return new RequestValidationException(
                    "Corporate action types list shouldn't be empty.", nameof(CorporateActionTypes));
            }

            if (TimeInterval.IsOpen())
            {
                yield return new RequestValidationException(
                    "You should specify both start and end of the interval.", nameof(TimeInterval));
            }
        }
    }
}
