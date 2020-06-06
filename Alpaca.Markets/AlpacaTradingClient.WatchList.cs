using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            _httpClient.GetObjectsListAsync<IWatchList, JsonWatchList>(
                "v2/watchlists", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Add new watch list object into Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">New watch list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Newly created watch list object.</returns>
        public async Task<IWatchList> CreateWatchListAsync(
            NewWatchListRequest request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(request.Name, request.Assets);
            using var response = await _httpClient.PostAsync(
                    new Uri("v2/watchlists", UriKind.RelativeOrAbsolute), content, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<IWatchList, JsonWatchList>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="watchListId"/> value.</returns>
        public Task<IWatchList> GetWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            _httpClient.GetSingleObjectAsync<IWatchList, JsonWatchList>(
                $"v2/watchlists/{watchListId:D}", cancellationToken, _alpacaRestApiThrottler);

        /// <summary>
        /// Get watch list object from Alpaca REST API endpoint by watch list user-defined name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Watch list object with proper <paramref name="name"/> value.</returns>
        public Task<IWatchList> GetWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default)
        {
            if (name.IsWatchListNameInvalid())
            {
                throw new ArgumentException("Watch list name should be from 1 to 64 characters length.", nameof(name));
            }

            return _httpClient.GetSingleObjectAsync<IWatchList, JsonWatchList>(
                new UriBuilder(_httpClient.BaseAddress)
                {
                    Path ="v2/watchlists:by_name",
                    Query = new QueryBuilder()
                        .AddParameter("name", name)
                }, 
                cancellationToken, _alpacaRestApiThrottler);
        }

        /// <summary>
        /// Updates watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Update watch list request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.WatchListId"/> value.</returns>
        public async Task<IWatchList> UpdateWatchListByIdAsync(
            UpdateWatchListRequest request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(request.Name, request.Assets);
            using var response = await _httpClient.PutAsync(
                    new Uri($"v2/watchlists/{request.WatchListId:D}", UriKind.RelativeOrAbsolute), content, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<IWatchList, JsonWatchList>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Asset adding request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public async Task<IWatchList> AddAssetIntoWatchListByIdAsync(
            ChangeWatchListRequest<Guid> request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(request.Asset);
            using var response = await _httpClient.PostAsync(
                    new Uri($"v2/watchlists/{request.Key:D}", UriKind.RelativeOrAbsolute), content, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<IWatchList, JsonWatchList>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="request">Asset adding request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public async Task<IWatchList> AddAssetIntoWatchListByNameAsync(
            ChangeWatchListRequest<String> request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v2/watchlists:by_name",
                Query = new QueryBuilder()
                    .AddParameter("name", request.Key)
            };

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(request.Asset);
            using var response = await _httpClient.PostAsync(builder.Uri, content, cancellationToken)
                .ConfigureAwait(false);

            return await response.DeserializeAsync<IWatchList, JsonWatchList>()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="request">Asset deleting request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public async Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
            ChangeWatchListRequest<Guid> request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            return await _httpClient.DeleteSingleObjectAsync<IWatchList, JsonWatchList>(
                    $"v2/watchlists/{request.Key:D}/{request.Asset}", cancellationToken, _alpacaRestApiThrottler)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="request">Asset deleting request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="request.Key"/> value.</returns>
        public async Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
            ChangeWatchListRequest<String> request,
            CancellationToken cancellationToken = default)
        {
            request.EnsureNotNull(nameof(request)).Validate();

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = $"v2/watchlists:by_name/{request.Asset}",
                Query = new QueryBuilder()
                    .AddParameter("name", request.Key)
            };

            return await _httpClient
                .DeleteSingleObjectAsync<IWatchList, JsonWatchList>(
                    builder, cancellationToken, _alpacaRestApiThrottler)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public async Task<Boolean> DeleteWatchListByIdAsync(
            Guid watchListId,
            CancellationToken cancellationToken = default) =>
            await _httpClient.DeleteAsync(
                    $"v2/watchlists/{watchListId:D}", cancellationToken, _alpacaRestApiThrottler)
                .ConfigureAwait(false);

        /// <summary>
        /// Deletes watch list from Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Returns <c>true</c> if operation completed successfully.</returns>
        public async Task<Boolean> DeleteWatchListByNameAsync(
            String name,
            CancellationToken cancellationToken = default)
        {
            if (name.IsWatchListNameInvalid())
            {
                throw new ArgumentException("Watch list name should be from 1 to 64 characters length.", nameof(name));
            }

            var builder = new UriBuilder(_httpClient.BaseAddress)
            {
                Path = "v2/watchlists:by_name",
                Query = new QueryBuilder()
                    .AddParameter("name", name)
            };

            return await _httpClient.DeleteAsync(
                    builder, cancellationToken, _alpacaRestApiThrottler)
                .ConfigureAwait(false);
        }


        private static StringContent toStringContent(
            String name,
            IEnumerable<String> assets) =>
            toStringContent(new
            {
                Name = name,
                Symbols = assets?.ToList()
            });

        private static StringContent toStringContent(String asset)
            => toStringContent(new { Symbol = asset });
    }
}
