using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class AlpacaTradingClient
    {
        /// <summary>
        /// Gets list of watch list objects from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of watch list objects.</returns>
        public Task<IReadOnlyList<IWatchList>> ListWatchListsAsync(
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IReadOnlyList<IWatchList>, List<JsonWatchList>>(
                "v2/watchlists", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Add new watch list object into Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">New watch list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Newly created watch list object.</returns>
        public Task<IWatchList> CreateWatchListAsync(
            NewWatchListRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.PostAsync<IWatchList, JsonWatchList, NewWatchListRequest>(
                "v2/watchlists", request,  cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> GetWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IWatchList, JsonWatchList>(
                getEndpointUri(watchListId), cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list user-defined name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> GetWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetAsync<IWatchList, JsonWatchList>(
                getEndpointUriBuilder(name), cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Updates watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Update watch list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.WatchListId"/> value.</returns>
        public Task<IWatchList> UpdateWatchListByIdAsync(
            UpdateWatchListRequest request,
            CancellationToken cancellationToken = default) =>
            _httpClient.PostAsync<IWatchList, JsonWatchList, UpdateWatchListRequest>(
                getEndpointUri(request.EnsureNotNull(nameof(request)).Validate().WatchListId), request,
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Asset adding request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public Task<IWatchList> AddAssetIntoWatchListByIdAsync(
            ChangeWatchListRequest<Guid> request,
            CancellationToken cancellationToken = default) =>
            _httpClient.PostAsync<IWatchList, JsonWatchList, ChangeWatchListRequest<Guid>>(
                getEndpointUri(request.EnsureNotNull(nameof(request)).Validate().Key), request,
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="request">Asset adding request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public Task<IWatchList> AddAssetIntoWatchListByNameAsync(
            ChangeWatchListRequest<String> request,
            CancellationToken cancellationToken = default) =>
            _httpClient.PostAsync<IWatchList, JsonWatchList, ChangeWatchListRequest<String>>(
                getEndpointUriBuilder(request.EnsureNotNull(nameof(request)).Validate().Key).Uri, request,
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Asset deleting request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
            ChangeWatchListRequest<Guid> request,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync<IWatchList, JsonWatchList>(
                getEndpointUri(request.EnsureNotNull(nameof(request)).Validate().Key, request.Asset),
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="request">Asset deleting request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
            ChangeWatchListRequest<String> request,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync<IWatchList, JsonWatchList>(
                getEndpointUriBuilder(request.EnsureNotNull(nameof(request)).Validate().Key, request.Asset),
                cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public Task<Boolean> DeleteWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync(
                getEndpointUri(watchListId), cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public Task<Boolean> DeleteWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default) =>
            _httpClient.DeleteAsync(
                getEndpointUriBuilder(name), cancellationToken, _alpacaRestApiThrottler);

        private UriBuilder getEndpointUriBuilder(
            String name,
            String? asset = null) =>
            new UriBuilder(_httpClient.BaseAddress)
            {
                Path = String.IsNullOrEmpty(asset)
                    ? "v2/watchlists:by_name"
                    : $"v2/watchlists:by_name/{asset}",
                Query = new QueryBuilder()
                    .AddParameter("name", 
                        name.ValidateWatchListName() ?? throw new ArgumentException(
                            "Watch list name should be from 1 to 64 characters length.", nameof(name)))
            };

        private static String getEndpointUri(
            Guid watchListId,
            String? asset = null) => 
            String.IsNullOrEmpty(asset)
                ? $"v2/watchlists/{watchListId:D}"
                : $"v2/watchlists/{watchListId:D}/{asset}";
    }
}
