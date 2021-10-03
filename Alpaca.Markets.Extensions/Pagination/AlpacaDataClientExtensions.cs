using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaDataClient"/> interface.
    /// </summary>
    public static partial class AlpacaDataClientExtensions
    {
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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).GetHistoricalBarsAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).GetHistoricalBarsAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).GetHistoricalQuotesAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).GetHistoricalQuotesAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

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
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);

        private static HistoricalBarsRequest getValidatedRequestWithoutPageToken(
            HistoricalBarsRequest request) =>
            new HistoricalBarsRequest(
                    request.Symbols,
                    request.GetValidatedFrom(),
                    request.GetValidatedInto(),
                    request.TimeFrame)
                .WithPageSize(request.GetPageSize());

        private static HistoricalQuotesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalQuotesRequest request) =>
            new HistoricalQuotesRequest(
                    request.Symbols,
                    request.GetValidatedFrom(),
                    request.GetValidatedInto())
                .WithPageSize(request.GetPageSize());

        private static HistoricalTradesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalTradesRequest request) =>
            new HistoricalTradesRequest(
                    request.Symbols,
                    request.GetValidatedFrom(),
                    request.GetValidatedInto())
                .WithPageSize(request.GetPageSize());
    }
}
