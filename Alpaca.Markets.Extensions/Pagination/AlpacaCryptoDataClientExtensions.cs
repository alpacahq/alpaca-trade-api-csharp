using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaCryptoDataClient"/> interface.
    /// </summary>
    public static partial class AlpacaCryptoDataClientExtensions
    {
        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IBar}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request) =>
            GetHistoricalBarsAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IBar}"/> interface) so they
        /// can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetHistoricalBarsAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IBar}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IBar>> GetHistoricalBarsDictionaryOfAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request) => 
            GetHistoricalBarsDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IBar}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IBar>> GetHistoricalBarsDictionaryOfAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).GetHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IBar>> GetHistoricalBarsPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request) =>
            GetHistoricalBarsPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IBar>> GetHistoricalBarsPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IBar>>> GetHistoricalBarsMultiPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request) =>
            GetHistoricalBarsMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalBarsAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IBar>>> GetHistoricalBarsMultiPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).GetHistoricalBarsAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request) =>
            GetHistoricalQuotesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IQuote}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IQuote> GetHistoricalQuotesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IQuote}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IQuote>> GetHistoricalQuotesDictionaryOfAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request) => 
            GetHistoricalQuotesDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IQuote}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<IQuote>> GetHistoricalQuotesDictionaryOfAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).GetHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request) =>
            GetHistoricalQuotesPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<IQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IQuote>>> GetHistoricalQuotesMultiPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request) =>
            GetHistoricalQuotesMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalQuotesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical quotes request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IQuote>>> GetHistoricalQuotesMultiPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoQuotesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).GetHistoricalQuotesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{ITrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request) =>
            GetHistoricalTradesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{ITrade}"/> interface)
        /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{ITrade}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<ITrade>> GetHistoricalTradesDictionaryOfAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request) => 
            GetHistoricalTradesDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.GetHistoricalTradesAsync"/> in pagination
        /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{ITrade}"/>
        /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns></returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IReadOnlyDictionary<String, IAsyncEnumerable<ITrade>> GetHistoricalTradesDictionaryOfAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByItems(client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request) =>
            GetHistoricalTradesPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<ITrade>>> GetHistoricalTradesMultiPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request) =>
            GetHistoricalTradesMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

        /// <summary>
        /// Gets all items provided by <see cref="IAlpacaCryptoDataClient.ListHistoricalTradesAsync"/> in pagination
        /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
        /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical trades request (with empty next page token).</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<ITrade>>> GetHistoricalTradesMultiPagesAsAsyncEnumerable(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoTradesRequest request,
            CancellationToken cancellationToken) =>
            getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
                .GetResponsesByPages(client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);

        private static HistoricalCryptoBarsRequest getValidatedRequestWithoutPageToken(
            HistoricalCryptoBarsRequest request) =>
            new HistoricalCryptoBarsRequest(
                    request.Symbols,
                    request.GetValidatedFrom(),
                    request.GetValidatedInto(),
                    request.TimeFrame)
                .WithPageSize(request.GetPageSize());

        private static HistoricalCryptoQuotesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalCryptoQuotesRequest request) =>
            new HistoricalCryptoQuotesRequest(
                    request.Symbols,
                    request.GetValidatedFrom(),
                    request.GetValidatedInto())
                .WithPageSize(request.GetPageSize());

        private static HistoricalCryptoTradesRequest getValidatedRequestWithoutPageToken(
            // ReSharper disable once SuggestBaseTypeForParameter
            HistoricalCryptoTradesRequest request) =>
            new HistoricalCryptoTradesRequest(
                    request.Symbols,
                    request.GetValidatedFrom(),
                    request.GetValidatedInto())
                .WithPageSize(request.GetPageSize());
    }
}