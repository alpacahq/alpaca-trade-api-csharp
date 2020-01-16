using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        /// <summary>
        /// Gets list of watch list objects from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of watch list objects.</returns>
        public Task<IReadOnlyList<IWatchList>> ListWatchListsAsync(
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.ListWatchListsAsync(cancellationToken);

        /// <summary>
        /// Add new watch list object into Alpaca REST API endpoint.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Newly created watch list object.</returns>
        public Task<IWatchList> CreateWatchListAsync(
            String name,
            IEnumerable<String>? assets = null,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.CreateWatchListAsync(name, assets, cancellationToken);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> GetWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetWatchListByIdAsync(watchListId, cancellationToken);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list user-defined name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> GetWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.GetWatchListByNameAsync(name, cancellationToken);

        /// <summary>
        /// Updates watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> UpdateWatchListByIdAsync(
            Guid watchListId,
            String name,
            IEnumerable<String> assets,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.UpdateWatchListByIdAsync(watchListId, name, assets, cancellationToken);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> AddAssetIntoWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.AddAssetIntoWatchListByIdAsync(watchListId, asset, cancellationToken);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> AddAssetIntoWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.AddAssetIntoWatchListByNameAsync(name, asset, cancellationToken);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteAssetFromWatchListByIdAsync(watchListId, asset, cancellationToken);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteAssetFromWatchListByNameAsync(name, asset, cancellationToken);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public Task<Boolean> DeleteWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteWatchListByIdAsync(watchListId, cancellationToken);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public Task<Boolean> DeleteWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default) =>
            _alpacaTradingClient.DeleteWatchListByNameAsync(name, cancellationToken);
    }
}
