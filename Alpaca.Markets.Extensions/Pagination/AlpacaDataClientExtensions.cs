using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataClient"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AlpacaDataClientExtensions
    {
        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalBar}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IHistoricalBar> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request) =>
            GetHistoricalBarsAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalBar}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IHistoricalBar> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken) =>
            getPaginatedResponsesAsAsyncEnumerable(
                new HistoricalBarsRequest(
                    request.EnsureNotNull(nameof(request)).Symbol,
                    request.TimeInterval.From ?? throw new ArgumentException(
                        "Invalid request time interval - empty start date", nameof(request)),
                    request.TimeInterval.Into ?? throw new ArgumentException(
                        "Invalid request time interval - empty end date", nameof(request)),
                    request.TimeFrame),
                client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IHistoricalQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request) =>
            GetHistoricalQuotesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IHistoricalQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken) =>
            getPaginatedResponsesAsAsyncEnumerable(
                new HistoricalQuotesRequest(
                    request.EnsureNotNull(nameof(request)).Symbol,
                    request.TimeInterval.From ?? throw new ArgumentException(
                        "Invalid request time interval - empty start date", nameof(request)),
                    request.TimeInterval.Into ?? throw new ArgumentException(
                        "Invalid request time interval - empty end date", nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalTrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IHistoricalTrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request) =>
            GetHistoricalTradesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalTrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IHistoricalTrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken) =>
            getPaginatedResponsesAsAsyncEnumerable(
                new HistoricalTradesRequest(
                    request.EnsureNotNull(nameof(request)).Symbol,
                    request.TimeInterval.From ?? throw new ArgumentException(
                        "Invalid request time interval - empty start date", nameof(request)),
                    request.TimeInterval.Into ?? throw new ArgumentException(
                        "Invalid request time interval - empty end date", nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

        private static async IAsyncEnumerable<TItem> getPaginatedResponsesAsAsyncEnumerable<TRequest, TItem>(
            TRequest singlePageOfItemsRequestWithEmptyPageToken,
            Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TRequest : HistoricalRequestBase
        {
            var request = singlePageOfItemsRequestWithEmptyPageToken.WithPageSize(10_000);
            do
            {
                var response = await getSinglePage(request, cancellationToken)
                    .ConfigureAwait(false);

                foreach (var item in response.Items)
                {
                    yield return item;
                }

                request.WithPageToken(response.NextPageToken ?? String.Empty);
            } while (!String.IsNullOrEmpty(request.Pagination.Token));
        }
    }
}
