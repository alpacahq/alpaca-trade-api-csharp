namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IHistoricalTradesClient{TRequest}"/> interface.
/// </summary>
public static class HistoricalTradesClientExtensions
{
    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{ITrade}"/> interface)
    /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        GetHistoricalTradesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{ITrade}"/> interface)
    /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull(nameof(request)).GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.GetHistoricalTradesAsync"/> in pagination
    /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{ITrade}"/>
    /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IReadOnlyDictionary<String, IAsyncEnumerable<ITrade>> GetHistoricalTradesDictionaryOfAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        GetHistoricalTradesDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.GetHistoricalTradesAsync"/> in pagination
    /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{ITrade}"/>
    /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IReadOnlyDictionary<String, IAsyncEnumerable<ITrade>> GetHistoricalTradesDictionaryOfAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull(nameof(request)).GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        GetHistoricalTradesPagesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull(nameof(request)).GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListHistoricalTradesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<ITrade>>>
        GetHistoricalTradesMultiPagesAsAsyncEnumerable<TRequest>(
            this IHistoricalTradesClient<TRequest> client,
            TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        GetHistoricalTradesMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<ITrade>>>
        GetHistoricalTradesMultiPagesAsAsyncEnumerable<TRequest>(
            this IHistoricalTradesClient<TRequest> client,
            TRequest request,
            CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull(nameof(request)).GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull(nameof(client)).GetHistoricalTradesAsync, cancellationToken);
}
