namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IHistoricalBarsClient{TRequest}"/> interface.
/// </summary>
public static partial class HistoricalBarsClientExtensions
{
    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.ListHistoricalBarsAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IBar}"/> interface) so they
    /// can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IBar> GetHistoricalBarsAsAsyncEnumerable<TRequest>(
        this IHistoricalBarsClient<TRequest> client,
        TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        GetHistoricalBarsAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.ListHistoricalBarsAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IBar}"/> interface) so they
    /// can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IBar> GetHistoricalBarsAsAsyncEnumerable<TRequest>(
        this IHistoricalBarsClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull().ListHistoricalBarsAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.GetHistoricalBarsAsync"/> in pagination
    /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IBar}"/>
    /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IReadOnlyDictionary<String, IAsyncEnumerable<IBar>>
        GetHistoricalBarsDictionaryOfAsyncEnumerable<TRequest>(
            this IHistoricalBarsClient<TRequest> client,
            TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        GetHistoricalBarsDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.GetHistoricalBarsAsync"/> in pagination
    /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IBar}"/>
    /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IReadOnlyDictionary<String, IAsyncEnumerable<IBar>>
        GetHistoricalBarsDictionaryOfAsyncEnumerable<TRequest>(
            this IHistoricalBarsClient<TRequest> client,
            TRequest request,
            CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull().GetHistoricalBarsAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.ListHistoricalBarsAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<IBar>> GetHistoricalBarsPagesAsAsyncEnumerable<TRequest>(
        this IHistoricalBarsClient<TRequest> client,
        TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        GetHistoricalBarsPagesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.ListHistoricalBarsAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<IBar>> GetHistoricalBarsPagesAsAsyncEnumerable<TRequest>(
        this IHistoricalBarsClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull().ListHistoricalBarsAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.GetHistoricalBarsAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IBar>>>
        GetHistoricalBarsMultiPagesAsAsyncEnumerable<TRequest>(
            this IHistoricalBarsClient<TRequest> client,
            TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        GetHistoricalBarsMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalBarsClient{TRequest}.GetHistoricalBarsAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IBar>>>
        GetHistoricalBarsMultiPagesAsAsyncEnumerable<TRequest>(
            this IHistoricalBarsClient<TRequest> client,
            TRequest request,
            CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull().GetHistoricalBarsAsync, cancellationToken);
}
