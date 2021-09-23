using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataClient"/> interface.
    /// </summary>
    public static partial class AlpacaDataClientExtensions
    {
        private const UInt32 MaxPageSize = 10_000;

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IBar}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
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
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IBar}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IBar>> GetHistoricalBarsDictionaryOfAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request) => 
            GetHistoricalBarsDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IBar}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IBar>> GetHistoricalBarsDictionaryOfAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).GetHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        [UsedImplicitly]
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
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IBar>> GetHistoricalBarsPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IBar>>> GetHistoricalBarsMultiPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request) =>
            GetHistoricalBarsMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IBar>>> GetHistoricalBarsMultiPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalBarsRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).GetHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
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
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IQuote}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IQuote>> GetHistoricalQuotesDictionaryOfAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request) => 
            GetHistoricalQuotesDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IQuote}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IQuote>> GetHistoricalQuotesDictionaryOfAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).GetHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        [UsedImplicitly]
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
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IQuote>>> GetHistoricalQuotesMultiPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request) =>
            GetHistoricalQuotesMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IQuote>>> GetHistoricalQuotesMultiPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalQuotesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).GetHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{ITrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
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
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{ITrade}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<ITrade>> GetHistoricalTradesDictionaryOfAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request) => 
            GetHistoricalTradesDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.GetHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{ITrade}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<ITrade>> GetHistoricalTradesDictionaryOfAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByItems(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        [UsedImplicitly]
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
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<ITrade>>> GetHistoricalTradesMultiPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request) =>
            GetHistoricalTradesMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<ITrade>>> GetHistoricalTradesMultiPagesAsAsyncEnumerable(
            this IAlpacaDataClient client,
            HistoricalTradesRequest request,
            CancellationToken cancellationToken) =>
            getResponsesByPages(
                getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request))),
                client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);

        private static HistoricalBarsRequest getValidatedRequestWithoutPageToken(
            HistoricalBarsRequest request) =>
            new HistoricalBarsRequest(
                    request.Symbols,
                    getValidatedFrom(request),
                    getValidatedInto(request),
                    request.TimeFrame)
                .WithPageSize(request.Pagination.Size ?? MaxPageSize);

        private static HistoricalQuotesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalQuotesRequest request) =>
            new HistoricalQuotesRequest(
                    request.Symbols,
                    getValidatedFrom(request),
                    getValidatedInto(request))
                .WithPageSize(request.Pagination.Size ?? MaxPageSize);

        private static HistoricalTradesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalTradesRequest request) =>
            new HistoricalTradesRequest(
                    request.Symbols,
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

        private static IReadOnlyDictionary<String, IAsyncEnumerable<TItem>> getResponsesByItems<TRequest, TItem>(
            TRequest singlePageOfItemsRequestWithEmptyPageToken,
            Func<TRequest, CancellationToken, Task<IMultiPage<TItem>>> getSinglePage,
            CancellationToken cancellationToken = default)
            where TRequest : HistoricalRequestBase
        {
            var channelsBySymbols =
                singlePageOfItemsRequestWithEmptyPageToken.Symbols
                    .ToDictionary(_ => _, _ => Channel.CreateUnbounded<TItem>(),
                        StringComparer.Ordinal);

            Task.Run(GetResponsesByItemsImpl, cancellationToken);

            return channelsBySymbols.ToDictionary(
                _ => _.Key, _ => ReadAllAsync(_.Value.Reader, cancellationToken),
                StringComparer.Ordinal);

            async Task GetResponsesByItemsImpl()
            {
                await foreach (var page in getResponsesByPages(
                        singlePageOfItemsRequestWithEmptyPageToken, getSinglePage, cancellationToken)
                    .ConfigureAwait(false))
                {
                    foreach (var kvp in page)
                    {
                        await WriteAllAsync(channelsBySymbols[kvp.Key].Writer, kvp.Value, cancellationToken)
                            .ConfigureAwait(false);
                    }
                }

                foreach (var channel in channelsBySymbols.Values)
                {
                    channel.Writer.TryComplete();
                }
            }

            static async IAsyncEnumerable<T> ReadAllAsync<T>(
                ChannelReader<T> reader,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            {
                while (await reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    while (reader.TryRead(out var item))
                    {
                        yield return item;
                    }
                }
            }

            static async ValueTask WriteAllAsync<T>(
                ChannelWriter<T> writer,
                IEnumerable<T> items,
                CancellationToken cancellationToken)
            {
                foreach (var item in items)
                {
                    await writer.WriteAsync(item, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private static IAsyncEnumerable<IReadOnlyList<TItem>> getResponsesByPages<TRequest, TItem>(
            TRequest singlePageOfItemsRequestWithEmptyPageToken,
            Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
            CancellationToken cancellationToken = default)
            where TRequest : HistoricalRequestBase =>
            getResponses(
                singlePageOfItemsRequestWithEmptyPageToken,
                (request, ct) => getItemsAndNextPageToken(getSinglePage, request, ct),
                cancellationToken);

        private static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<TItem>>> getResponsesByPages<TRequest, TItem>(
            TRequest singlePageOfItemsRequestWithEmptyPageToken,
            Func<TRequest, CancellationToken, Task<IMultiPage<TItem>>> getSinglePage,
            CancellationToken cancellationToken = default)
            where TRequest : HistoricalRequestBase =>
            getResponses(
                singlePageOfItemsRequestWithEmptyPageToken,
                (request, ct) => getItemsAndNextPageToken(getSinglePage, request, ct),
                cancellationToken);

        private static async IAsyncEnumerable<TResponse> getResponses<TRequest, TResponse>(
            TRequest singlePageOfItemsRequestWithEmptyPageToken,
            Func<TRequest, CancellationToken, Task<(TResponse, String?)>> getItemsAndNextPageToken,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TRequest : HistoricalRequestBase
        {
            var request = singlePageOfItemsRequestWithEmptyPageToken;
            do
            {
                var (items, nextPageToken) = await getItemsAndNextPageToken(
                    request, cancellationToken).ConfigureAwait(false);

                yield return items;

                request = request.WithPageToken(nextPageToken ?? String.Empty);
            } while (!String.IsNullOrEmpty(request.Pagination.Token));
        }

        private static async Task<(IReadOnlyList<TItem>, String?)> getItemsAndNextPageToken<TRequest, TItem>(
            this Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
            TRequest request,
            CancellationToken cancellationToken)
        {
            var response = await getSinglePage(request, cancellationToken).ConfigureAwait(false);
            return (response.Items, response.NextPageToken);
        }

        private static async Task<(IReadOnlyDictionary<String, IReadOnlyList<TItem>>, String?)> getItemsAndNextPageToken<TRequest, TItem>(
            this Func<TRequest, CancellationToken, Task<IMultiPage<TItem>>> getSinglePage,
            TRequest request,
            CancellationToken cancellationToken)
        {
            var response = await getSinglePage(request, cancellationToken).ConfigureAwait(false);
            return (response.Items, response.NextPageToken);
        }
    }
}
