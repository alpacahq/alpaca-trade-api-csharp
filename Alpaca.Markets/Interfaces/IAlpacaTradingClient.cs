using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Trading API via HTTP/REST.
    /// </summary>
    public interface IAlpacaTradingClient : IDisposable
    {
        /// <summary>
        /// Gets list of watch list objects from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of watch list objects.</returns>
        Task<IReadOnlyList<IWatchList>> ListWatchListsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new watch list object into Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">New watch list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Newly created watch list object.</returns>
        Task<IWatchList> CreateWatchListAsync(
            NewWatchListRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="watchListId"/> value.</returns>
        Task<IWatchList> GetWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list user-defined name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="name"/> value.</returns>
        Task<IWatchList> GetWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Update watch list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.WatchListId"/> value.</returns>
        Task<IWatchList> UpdateWatchListByIdAsync(
            UpdateWatchListRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Asset adding request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        Task<IWatchList> AddAssetIntoWatchListByIdAsync(
            ChangeWatchListRequest<Guid> request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="request">Asset adding request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        Task<IWatchList> AddAssetIntoWatchListByNameAsync(
            ChangeWatchListRequest<String> request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Asset deleting request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
            ChangeWatchListRequest<Guid> request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="request">Asset deleting request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
            ChangeWatchListRequest<String> request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        Task<Boolean> DeleteWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        Task<Boolean> DeleteWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of available orders from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">List orders request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of order information objects.</returns>
        Task<IReadOnlyList<IOrder>> ListOrdersAsync(
            ListOrdersRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates new order for execution using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">New order placement request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for newly created order.</returns>
        Task<IOrder> PostOrderAsync(
            NewOrderRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates new order for execution using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderBase">New order placement request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for newly created order.</returns>
        Task<IOrder> PostOrderAsync(
            OrderBase orderBase,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates existing order using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Patch order request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object for updated order.</returns>
        Task<IOrder> PatchOrderAsync(
            ChangeOrderRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get single order information by client order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="clientOrderId">Client order ID for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object.</returns>
        Task<IOrder> GetOrderAsync(
            String clientOrderId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get single order information by server order ID from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only order information object.</returns>
        Task<IOrder> GetOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes/cancel order on server by server order ID using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="orderId">Server order ID for cancelling.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>True</c> if order cancellation was accepted.</returns>
        Task<Boolean> DeleteOrderAsync(
            Guid orderId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes/cancel all open orders using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of order cancellation status objects.</returns>
        Task<IReadOnlyList<IOrderActionStatus>> DeleteAllOrdersAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets account information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only account information.</returns>
        Task<IAccount> GetAccountAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets account configuration settings from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of account configuration object.</returns>
        Task<IAccountConfiguration> GetAccountConfigurationAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates account configuration settings using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="accountConfiguration">New account configuration object for updating.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Mutable version of updated account configuration object.</returns>
        Task<IAccountConfiguration> PatchAccountConfigurationAsync(
            IAccountConfiguration accountConfiguration,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of account activities from Alpaca REST API endpoint by specific activity.
        /// </summary>
        /// <param name="request">Account activities request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of account activity record objects.</returns>
        Task<IReadOnlyList<IAccountActivity>> ListAccountActivitiesAsync(
            AccountActivitiesRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets portfolio equity history from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Portfolio history request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only portfolio history information object.</returns>
        Task<IPortfolioHistory> GetPortfolioHistoryAsync(
            PortfolioHistoryRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of available assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Asset list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of asset information objects.</returns>
        Task<IReadOnlyList<IAsset>> ListAssetsAsync(
            AssetsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get single asset information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Asset name for searching.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only asset information.</returns>
        Task<IAsset> GetAssetAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of available positions from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of position information objects.</returns>
        Task<IReadOnlyList<IPosition>> ListPositionsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets position information by asset name from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="symbol">Position asset name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only position information object.</returns>
        Task<IPosition> GetPositionAsync(
            String symbol,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Liquidates all open positions at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of position cancellation status objects.</returns>
        Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Liquidates all open positions at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">All positions deletion request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>List of position cancellation status objects.</returns>
        Task<IReadOnlyList<IPositionActionStatus>> DeleteAllPositionsAsync(
            DeleteAllPositionsRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Liquidate an open position at market price using Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Position deletion request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The <see cref="IOrder"/> object that represents the position liquidation order (for tracking).</returns>
        Task<IOrder> DeletePositionAsync(
            DeletePositionRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get current time information from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only clock information object.</returns>
        Task<IClock> GetClockAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets list of trading days from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Calendar items request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of trading date information object.</returns>
        Task<IReadOnlyList<ICalendar>> ListCalendarAsync(
            CalendarRequest request,
            CancellationToken cancellationToken = default);
    }
}
