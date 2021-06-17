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
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class AlpacaDataClientExtensions
    {
        private const UInt32 MaxPageSize = 10_000;

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IBar}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request) =>
            GetHistoricalBarsAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IBar}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken) =>
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
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IBar>> GetHistoricalBarsPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request) =>
            GetHistoricalBarsPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IBar>> GetHistoricalBarsPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request) =>
            GetHistoricalQuotesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken) =>
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
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request) =>
            GetHistoricalQuotesPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{ITrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request) =>
            GetHistoricalTradesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{ITrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken) =>
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
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request) =>
            GetHistoricalTradesPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken) =>
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
            HistoricalQuotesRequest request) =>
            new HistoricalQuotesRequest(
                    request.Symbol,
                    getValidatedFrom(request),
                    getValidatedInto(request))
                .WithPageSize(request.Pagination.Size ?? MaxPageSize);

        private static HistoricalTradesRequest getValidatedRequestWithoutPageToken(
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
