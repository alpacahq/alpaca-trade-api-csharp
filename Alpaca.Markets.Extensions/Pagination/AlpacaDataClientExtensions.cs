using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    using static Pagination;

    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataClient"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AlpacaDataClientExtensions
    {
        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IAgg}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public static IAsyncEnumerable<IAgg> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public static IAsyncEnumerable<IReadOnlyList<IAgg>> GetHistoricalBarsPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken = default) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public static IAsyncEnumerable<IHistoricalQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken = default) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public static IAsyncEnumerable<IReadOnlyList<IHistoricalQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken = default) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IHistoricalTrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public static IAsyncEnumerable<IHistoricalTrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken = default) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        public static IAsyncEnumerable<IReadOnlyList<IHistoricalTrade>> GetHistoricalTradesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken = default) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

        private static HistoricalBarsRequest getValidatedRequestWithoutPageToken(
            HistoricalBarsRequest request) =>
            new HistoricalBarsRequest(
                    request.Symbol,
                    getValidatedFrom(request),
                    getValidatedInto(request),
                    request.TimeFrame)
                .WithPageSize(request.Pagination.Size ?? MaxPageSize);

        private static HistoricalQuotesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalQuotesRequest request) =>
            new HistoricalQuotesRequest(
                    request.Symbol,
                    getValidatedFrom(request),
                    getValidatedInto(request))
                .WithPageSize(request.Pagination.Size ?? MaxPageSize);

        private static HistoricalTradesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalTradesRequest request) =>
            new HistoricalTradesRequest(
                    request.Symbol,
                    getValidatedFrom(request),
                    getValidatedInto(request))
                .WithPageSize(request.Pagination.Size ?? MaxPageSize);

        private static DateTime getValidatedFrom(
            HistoricalRequestBase request) =>
            getValidatedDate(request.TimeInterval.From, nameof(request.TimeInterval.From));

        private static DateTime getValidatedInto(
            HistoricalRequestBase request) =>
            getValidatedDate(request.TimeInterval.Into, nameof(request.TimeInterval.Into));

        private static DateTime getValidatedDate(
            DateTime? date,
            String paramName) =>
            date ?? throw new ArgumentException(
                "Invalid request time interval - empty date", paramName);

        private static async IAsyncEnumerable<TItem> getResponsesByItems<TRequest, TItem>(
            TRequest singlePageOfItemsRequestWithEmptyPageToken,
            Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TRequest : HistoricalRequestBase
        {
            await foreach (var page in getResponsesByPages(
                singlePageOfItemsRequestWithEmptyPageToken, getSinglePage, cancellationToken)
                .ConfigureAwait(false))
            {
                foreach (var item in page)
                {
                    yield return item;
                }
            }
        }

        private static async IAsyncEnumerable<IReadOnlyList<TItem>> getResponsesByPages<TRequest, TItem>(
            TRequest singlePageOfItemsRequestWithEmptyPageToken,
            Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TRequest : HistoricalRequestBase
        {
            // ReSharper disable once InlineTemporaryVariable
            var request = singlePageOfItemsRequestWithEmptyPageToken;
            do
            {
                var response = await getSinglePage(request, cancellationToken)
                    .ConfigureAwait(false);

                yield return response.Items;

                request.WithPageToken(response.NextPageToken ?? String.Empty);
            } while (!String.IsNullOrEmpty(request.Pagination.Token));
        }
    }
}
