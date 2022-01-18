namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IAlpacaDataClient"/> interface.
/// </summary>
public static class AlpacaDataClientExtensions
{
    /// <summary>
    /// Gets all items provided by <see cref="IAlpacaDataClient.ListNewsArticlesAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{INewsArticle}"/> interface) so they
    /// can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <returns></returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<INewsArticle> GetNewsArticlesAsAsyncEnumerable(
        this IAlpacaDataClient client,
        NewsArticlesRequest request) =>
        GetNewsArticlesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IAlpacaDataClient.ListNewsArticlesAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{INewsArticle}"/> interface) so they
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
    public static IAsyncEnumerable<INewsArticle> GetNewsArticlesAsAsyncEnumerable(
        this IAlpacaDataClient client,
        NewsArticlesRequest request,
        CancellationToken cancellationToken) =>
        getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
            .GetResponsesByItems(client.EnsureNotNull(nameof(client)).ListNewsArticlesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IAlpacaDataClient.ListNewsArticlesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
    /// <param name="request">Original historical news articles request (with empty next page token).</param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<INewsArticle>> GetNewsArticlesPagesAsAsyncEnumerable(
        this IAlpacaDataClient client,
        NewsArticlesRequest request) =>
        GetNewsArticlesPagesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IAlpacaDataClient.ListNewsArticlesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataClient"/> interface.</param>
    /// <param name="request">Original historical news articles request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<INewsArticle>> GetNewsArticlesPagesAsAsyncEnumerable(
        this IAlpacaDataClient client,
        NewsArticlesRequest request,
        CancellationToken cancellationToken) =>
        getValidatedRequestWithoutPageToken(request.EnsureNotNull(nameof(request)))
            .GetResponsesByPages(client.EnsureNotNull(nameof(client)).ListNewsArticlesAsync, cancellationToken);

    private static NewsArticlesRequest getValidatedRequestWithoutPageToken(
        IHistoricalRequest<NewsArticlesRequest, INewsArticle> request) =>
        request.GetValidatedRequestWithoutPageToken();
}
