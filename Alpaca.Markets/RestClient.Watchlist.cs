using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        public Task<IEnumerable<IWatchList>> ListWatchListsAsync(
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "watchlists"
            };

            return getObjectsListAsync<IWatchList, JsonWatchList>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Add new watch list object into Alpaca REST API endpoint.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Newly created watch list object.</returns>
        public async Task<IWatchList> CreateWatchListAsync(
            String name,
            IEnumerable<String> assets = null,
            CancellationToken cancellationToken = default)
        {
            verifyWatchListName(name);

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(name, assets);
            using var response = await _alpacaHttpClient.PostAsync(
                    new Uri("watchlists", UriKind.RelativeOrAbsolute), content, cancellationToken)
                .ConfigureAwait(false);

            return await deserializeAsync<IWatchList, JsonWatchList>(response)
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
            CancellationToken cancellationToken = default)
        {
            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + $"watchlists/{watchListId:D}"
            };

            return getSingleObjectAsync<IWatchList, JsonWatchList>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder, cancellationToken);
        }

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
            verifyWatchListName(name);

            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "watchlists:by_name",
                Query = new QueryBuilder()
                    .AddParameter("name", name)
            };

            return getSingleObjectAsync<IWatchList, JsonWatchList>(
                _alpacaHttpClient, _alpacaRestApiThrottler, builder, cancellationToken);
        }

        /// <summary>
        /// Updates watch list object from Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public async Task<IWatchList> UpdateWatchListByIdAsync(
            Guid watchListId,
            String name,
            IEnumerable<String> assets,
            CancellationToken cancellationToken = default)
        {
            verifyWatchListName(name);

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(name, assets);
            using var response = await _alpacaHttpClient.PutAsync(
                    new Uri($"watchlists/{watchListId:D}", UriKind.RelativeOrAbsolute), content, cancellationToken)
                .ConfigureAwait(false);

            return await deserializeAsync<IWatchList, JsonWatchList>(response)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public async Task<IWatchList> AddAssetIntoWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default)
        {
            asset = asset ?? throw new ArgumentNullException(nameof(asset));

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(asset);
            using var response = await _alpacaHttpClient.PostAsync(
                    new Uri($"watchlists/{watchListId:D}", UriKind.RelativeOrAbsolute), content, cancellationToken)
                .ConfigureAwait(false);

            return await deserializeAsync<IWatchList, JsonWatchList>(response)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Adds asset into watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        public async Task<IWatchList> AddAssetIntoWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default)
        {
            asset = asset ?? throw new ArgumentNullException(nameof(asset));

            verifyWatchListName(name);

            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "watchlists:by_name",
                Query = new QueryBuilder()
                    .AddParameter("name", name)
            };

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var content = toStringContent(asset);
            using var response = await _alpacaHttpClient.PostAsync(builder.Uri, content, cancellationToken)
                .ConfigureAwait(false);

            return await deserializeAsync<IWatchList, JsonWatchList>(response)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list identifier.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="watchListId"/> value.</returns>
        public async Task<IWatchList> DeleteAssetFromWatchListByIdAsync(
            Guid watchListId,
            String asset,
            CancellationToken cancellationToken = default)
        {
            asset = asset ?? throw new ArgumentNullException(nameof(asset));

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var response = await _alpacaHttpClient.DeleteAsync(
                    new Uri($"watchlists/{watchListId:D}/{asset}", UriKind.RelativeOrAbsolute), cancellationToken)
                .ConfigureAwait(false);

            return await deserializeAsync<IWatchList, JsonWatchList>(response)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes asset from watch list using Alpaca REST API endpoint by watch list name.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Updated watch list object with proper <paramref name="name"/> value.</returns>
        public async Task<IWatchList> DeleteAssetFromWatchListByNameAsync(
            String name,
            String asset,
            CancellationToken cancellationToken = default)
        {
            asset = asset ?? throw new ArgumentNullException(nameof(asset));

            verifyWatchListName(name);

            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + $"watchlists:by_name/{asset}",
                Query = new QueryBuilder()
                    .AddParameter("name", name)
            };

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var response = await _alpacaHttpClient.DeleteAsync(builder.Uri, cancellationToken)
                .ConfigureAwait(false);

            return await deserializeAsync<IWatchList, JsonWatchList>(response)
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
            CancellationToken cancellationToken = default)
        {
            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var response = await _alpacaHttpClient.DeleteAsync(
                    new Uri($"watchlists/{watchListId:D}", UriKind.RelativeOrAbsolute), cancellationToken)
                .ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

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
            verifyWatchListName(name);

            var builder = new UriBuilder(_alpacaHttpClient.BaseAddress)
            {
                Path = _alpacaHttpClient.BaseAddress.AbsolutePath + "watchlists:by_name",
                Query = new QueryBuilder()
                    .AddParameter("name", name)
            };

            await _alpacaRestApiThrottler.WaitToProceed(cancellationToken).ConfigureAwait(false);

            using var response = await _alpacaHttpClient.DeleteAsync(builder.Uri, cancellationToken)
                .ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        private static void verifyWatchListName(String name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            if (name.Length == 0 ||
                name.Length > 64)
            {
                throw new ArgumentException(
                    "Watch list name should be from 1 to 64 symbols.", nameof(name));
            }
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
