using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint by specific activity.
        /// </summary>
        /// <param name="activityType">The activity type you want to view entries for.</param>
        /// <param name="date">The date for which you want to see activities.</param>
        /// <param name="until">The response will contain only activities submitted before this date. (Cannot be used with date.)</param>
        /// <param name="after">The response will contain only activities submitted after this date. (Cannot be used with date.)</param>
        /// <param name="direction">The response will be sorted by time in this direction. (Default behavior is descending.)</param>
        /// <param name="pageSize">The maximum number of entries to return in the response.</param>
        /// <param name="pageToken">The ID of the end of your current page of results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of account activity record objects.</returns>
        [Obsolete("Use overloaded method that required AccountActivitiesRequest parameter instead of this one.", false)]
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            AccountActivityType activityType,
            DateTime? date = null,
            DateTime? until = null,
            DateTime? after = null,
            SortDirection? direction = null,
            Int64? pageSize = null,
            String? pageToken = null,
            CancellationToken cancellationToken = default) =>
            ListAccountActivitiesAsync(
                new AccountActivitiesRequest(activityType)
                    {
                        Direction = direction,
                        PageSize = pageSize,
                        PageToken = pageToken
                    }
                    .SetTimes(date, after, until),
                cancellationToken);

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="activityTypes">The list of activity types you want to view entries for.</param>
        /// <param name="date">The date for which you want to see activities.</param>
        /// <param name="until">The response will contain only activities submitted before this date. (Cannot be used with date.)</param>
        /// <param name="after">The response will contain only activities submitted after this date. (Cannot be used with date.)</param>
        /// <param name="direction">The response will be sorted by time in this direction. (Default behavior is descending.)</param>
        /// <param name="pageSize">The maximum number of entries to return in the response.</param>
        /// <param name="pageToken">The ID of the end of your current page of results.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of account activity record objects.</returns>
        [Obsolete("Use overloaded method that required AccountActivitiesRequest parameter instead of this one.", false)]
        public Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            IEnumerable<AccountActivityType>? activityTypes = null,
            DateTime? date = null,
            DateTime? until = null,
            DateTime? after = null,
            SortDirection? direction = null,
            Int64? pageSize = null,
            String? pageToken = null,
            CancellationToken cancellationToken = default) =>
            ListAccountActivitiesAsync(
                new AccountActivitiesRequest(
                        activityTypes ?? Enumerable.Empty<AccountActivityType>())
                    {
                        Direction = direction,
                        PageSize = pageSize,
                        PageToken = pageToken
                    }
                    .SetTimes(date, after, until),
                cancellationToken);

        /// <summary>
        /// Gets portfolio equity history from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="startDate">Start date for desired history.</param>
        /// <param name="endDate">End date for desired history. Default value is today. </param>
        /// <param name="period">Period value for desired history. Default value is 1 month.</param>
        /// <param name="timeFrame">
        /// Time frame value for desired history.
        /// Default value is 1 minute for a period shorter than 7 days, 15 minutes for a period less than 30 days, or 1 day for a longer period.
        /// </param>
        /// <param name="extendedHours">If true, include extended hours in the result. This is effective only for time frame less than 1 day.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only portfolio history information object.</returns>
        [Obsolete("Use overloaded method that required PortfolioHistoryRequest parameter instead of this one.", false)]
        public Task<IPortfolioHistory> GetPortfolioHistoryAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            TimeFrame? timeFrame = null,
            HistoryPeriod? period = null,
            Boolean? extendedHours = null,
            CancellationToken cancellationToken = default) =>
            GetPortfolioHistoryAsync(
                new PortfolioHistoryRequest()
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    ExtendedHours = extendedHours,
                    Period = period,
                    TimeFrame = timeFrame
                },
                cancellationToken);

        /// <summary>
        /// Gets list of available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="assetStatus">Asset status for filtering.</param>
        /// <param name="assetClass">Asset class for filtering.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        [Obsolete("Use overloaded method that required AssetsRequest parameter instead of this one.", false)]
        public Task<IReadOnlyList<IAsset>> ListAssetsAsync(
            AssetStatus? assetStatus = null,
            AssetClass? assetClass = null,
            CancellationToken cancellationToken = default) =>
            ListAssetsAsync(
                new AssetsRequest
                {
                    AssetClass = assetClass,
                    AssetStatus = assetStatus
                },
                cancellationToken);

        /// <summary>
        /// Gets list of trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="startDateInclusive">Start time for filtering (inclusive).</param>
        /// <param name="endDateInclusive">End time for filtering (inclusive).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of trading date information object.</returns>
        [Obsolete("Use overloaded method that required CalendarRequest parameter instead of this one.", false)]
        public Task<IReadOnlyList<ICalendar>> ListCalendarAsync(
            DateTime? startDateInclusive = null,
            DateTime? endDateInclusive = null,
            CancellationToken cancellationToken = default) =>
            ListCalendarAsync(
                new CalendarRequest()
                    .SetInclusiveTimeInterval(startDateInclusive, endDateInclusive),
                cancellationToken);

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderStatusFilter">Order status for filtering.</param>
        /// <param name="orderListSorting">The chronological order of response based on the submission time.</param>
        /// <param name="untilDateTimeExclusive">Returns only orders until specified timestamp (exclusive).</param>
        /// <param name="afterDateTimeExclusive">Returns only orders after specified timestamp (exclusive).</param>
        /// <param name="limitOrderNumber">Maximal number of orders in response.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of order information objects.</returns>
        [Obsolete("Use overloaded method that required ListOrdersRequest parameter instead of this one.", false)]
        public Task<IReadOnlyList<IOrder>> ListOrdersAsync(
            OrderStatusFilter? orderStatusFilter = null,
            SortDirection? orderListSorting = null,
            DateTime? untilDateTimeExclusive = null,
            DateTime? afterDateTimeExclusive = null,
            Int64? limitOrderNumber = null,
            CancellationToken cancellationToken = default) =>
            ListOrdersAsync(
                new ListOrdersRequest
                    {
                        LimitOrderNumber = limitOrderNumber,
                        OrderListSorting = orderListSorting,
                        OrderStatusFilter = orderStatusFilter
                    }
                    .SetExclusiveTimeInterval(afterDateTimeExclusive, untilDateTimeExclusive),
                cancellationToken);

        /// <summary>
        /// Updates existing order using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server side order identifier.</param>
        /// <param name="quantity">Updated order quantity or <c>null</c> if quantity is not changed.</param>
        /// <param name="duration">Updated order duration or <c>null</c> if duration is not changed.</param>
        /// <param name="limitPrice">Updated order limit price or <c>null</c> if limit price is not changed.</param>
        /// <param name="stopPrice">Updated order stop price or <c>null</c> if stop price is not changed.</param>
        /// <param name="clientOrderId">Updated client order ID or <c>null</c> if client order ID is not changed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for updated order.</returns>
        [Obsolete("Use overloaded method that required PatchOrderRequest parameter instead of this one.", false)]
        public Task<IOrder> PatchOrderAsync(
            Guid orderId,
            Int64? quantity = null,
            TimeInForce? duration = null,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String? clientOrderId = null,
            CancellationToken cancellationToken = default) =>
            PatchOrderAsync(
                new PatchOrderRequest(orderId)
                {
                    Quantity = quantity,
                    Duration = duration,
                    StopPrice = stopPrice,
                    LimitPrice = limitPrice,
                    ClientOrderId = clientOrderId
                },
                cancellationToken);
    }
}
