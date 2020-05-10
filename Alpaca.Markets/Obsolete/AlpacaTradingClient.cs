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
        [Obsolete("Use overloaded method that required AccountActivitiesRequest parameter instead of this one.", true)]
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
        [Obsolete("Use overloaded method that required AccountActivitiesRequest parameter instead of this one.", true)]
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
        [Obsolete("Use overloaded method that required PortfolioHistoryRequest parameter instead of this one.", true)]
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
                    ExtendedHours = extendedHours,
                    Period = period,
                    TimeFrame = timeFrame
                }.SetTimeInterval(
                    TimeInterval.GetInclusive(startDate, endDate)),
                cancellationToken);

        /// <summary>
        /// Gets list of available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="assetStatus">Asset status for filtering.</param>
        /// <param name="assetClass">Asset class for filtering.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        [Obsolete("Use overloaded method that required AssetsRequest parameter instead of this one.", true)]
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
        [Obsolete("Use overloaded method that required CalendarRequest parameter instead of this one.", true)]
        public Task<IReadOnlyList<ICalendar>> ListCalendarAsync(
            DateTime? startDateInclusive = null,
            DateTime? endDateInclusive = null,
            CancellationToken cancellationToken = default) =>
            ListCalendarAsync(
                new CalendarRequest()
                    .SetInclusiveTimeIntervalWithNulls(startDateInclusive, endDateInclusive),
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
        [Obsolete("Use overloaded method that required ListOrdersRequest parameter instead of this one.", true)]
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
                    .SetExclusiveTimeIntervalWithNulls(afterDateTimeExclusive, untilDateTimeExclusive),
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
        [Obsolete("Use overloaded method that required ChangeOrderRequest parameter instead of this one.", true)]
        public Task<IOrder> PatchOrderAsync(
            Guid orderId,
            Int64? quantity = null,
            TimeInForce? duration = null,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String? clientOrderId = null,
            CancellationToken cancellationToken = default) =>
            PatchOrderAsync(
                new ChangeOrderRequest(orderId)
                {
                    Quantity = quantity,
                    Duration = duration,
                    StopPrice = stopPrice,
                    LimitPrice = limitPrice,
                    ClientOrderId = clientOrderId
                },
                cancellationToken);

        /// <summary>
        /// Creates new order for execution using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Order asset name.</param>
        /// <param name="quantity">Order quantity.</param>
        /// <param name="side">Order side (buy or sell).</param>
        /// <param name="type">Order type.</param>
        /// <param name="duration">Order duration.</param>
        /// <param name="limitPrice">Order limit price.</param>
        /// <param name="stopPrice">Order stop price.</param>
        /// <param name="clientOrderId">Client order ID.</param>
        /// <param name="extendedHours">Whether or not this order should be allowed to execute during extended hours trading.</param>
        /// <param name="orderClass">Order class for advanced order types.</param>
        /// <param name="takeProfitLimitPrice">Profit taking limit price for advanced order types.</param>
        /// <param name="stopLossStopPrice">Stop loss stop price for advanced order types.</param>
        /// <param name="stopLossLimitPrice">Stop loss limit price for advanced order types.</param>
        /// <param name="nested">Whether or not child orders should be listed as 'legs' of parent orders. (Advanced order types only.)</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for newly created order.</returns>
        [Obsolete("Use overloaded method that required NewOrderRequest parameter instead of this one.", true)]
        public Task<IOrder> PostOrderAsync(
            String symbol,
            Int64 quantity,
            OrderSide side,
            OrderType type,
            TimeInForce duration,
            Decimal? limitPrice = null,
            Decimal? stopPrice = null,
            String? clientOrderId = null,
            Boolean? extendedHours = null,
            OrderClass? orderClass = null,
            Decimal? takeProfitLimitPrice = null,
            Decimal? stopLossStopPrice = null,
            Decimal? stopLossLimitPrice = null,
            Boolean? nested = false,
            CancellationToken cancellationToken = default) =>
            PostOrderAsync(
                new NewOrderRequest(
                    symbol, quantity, side, type, duration)
                {
                    Nested = nested,
                    StopPrice = stopPrice,
                    LimitPrice = limitPrice,
                    OrderClass = orderClass,
                    ClientOrderId = clientOrderId,
                    ExtendedHours = extendedHours,
                    StopLossLimitPrice = stopLossLimitPrice,
                    StopLossStopPrice = stopLossStopPrice,
                    TakeProfitLimitPrice = takeProfitLimitPrice
                },
                cancellationToken);

        /// <summary>
        /// Add new watch list object into Alpaca REST API endpoint.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Newly created watch list object.</returns>
        [Obsolete("Use overloaded method that required NewWatchListRequest parameter instead of this one.", true)]
        public Task<IWatchList> CreateWatchListAsync(
            String name,
            IEnumerable<String>? assets = null,
            CancellationToken cancellationToken = default) =>
            CreateWatchListAsync(
                new NewWatchListRequest(
                    name, assets ?? Enumerable.Empty<String>()),
                cancellationToken);

        /// <summary>
        /// Updates watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        [Obsolete("Use overloaded method that required UpdateWatchListRequest parameter instead of this one.", true)]
        public Task<IWatchList> UpdateWatchListByIdAsync(
            Guid watchListId,
            String name,
            IEnumerable<String> assets,
            CancellationToken cancellationToken = default) =>
            UpdateWatchListByIdAsync(
                new UpdateWatchListRequest(
                    watchListId, name, assets), 
                cancellationToken);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        [Obsolete("Use overloaded method that required ChangeWatchListRequest parameter instead of this one.", true)]
        public Task<IWatchList> AddAssetIntoWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default) =>
            AddAssetIntoWatchListByIdAsync(
                new ChangeWatchListRequest<Guid>(
                    watchListId, asset),
                cancellationToken);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        [Obsolete("Use overloaded method that required ChangeWatchListRequest parameter instead of this one.", true)]
        public Task<IWatchList> AddAssetIntoWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default) =>
            AddAssetIntoWatchListByNameAsync(
                new ChangeWatchListRequest<String>(
                    name, asset),
                cancellationToken);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        [Obsolete("Use overloaded method that required ChangeWatchListRequest parameter instead of this one.", true)]
        public Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default) =>
            DeleteAssetFromWatchListByIdAsync(
                new ChangeWatchListRequest<Guid>(
                    watchListId, asset),
                cancellationToken);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        [Obsolete("Use overloaded method that required ChangeWatchListRequest parameter instead of this one.", true)]
        public Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default) =>
            DeleteAssetFromWatchListByNameAsync(
                new ChangeWatchListRequest<string>(
                    name, asset),
                cancellationToken);
    }
}
