namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.
/// </summary>
public static class HistoricalQuotesClientExtensions
{
    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IQuote}"/> interface)
    /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical quotes request (with empty next page token).</param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IQuote> GetHistoricalQuotesAsAsyncEnumerable<TRequest>(
        this IHistoricalQuotesClient<TRequest> client,
        TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        GetHistoricalQuotesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of items (in form of <see cref="IAsyncEnumerable{IQuote}"/> interface)
    /// so they can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical quotes request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IQuote> GetHistoricalQuotesAsAsyncEnumerable<TRequest>(
        this IHistoricalQuotesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull().ListHistoricalQuotesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.GetHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IQuote}"/>
    /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IReadOnlyDictionary<String, IAsyncEnumerable<IQuote>>
        GetHistoricalQuotesDictionaryOfAsyncEnumerable<TRequest>(
            this IHistoricalQuotesClient<TRequest> client,
            TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        GetHistoricalQuotesDictionaryOfAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.GetHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{IQuote}"/>
    /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical minute bars request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IReadOnlyDictionary<String, IAsyncEnumerable<IQuote>>
        GetHistoricalQuotesDictionaryOfAsyncEnumerable<TRequest>(
            this IHistoricalQuotesClient<TRequest> client,
            TRequest request,
            CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull().GetHistoricalQuotesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical quotes request (with empty next page token).</param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<IQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable<TRequest>(
        this IHistoricalQuotesClient<TRequest> client,
        TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        GetHistoricalQuotesPagesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical quotes request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyList<IQuote>> GetHistoricalQuotesPagesAsAsyncEnumerable<TRequest>(
        this IHistoricalQuotesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull().ListHistoricalQuotesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical quotes request (with empty next page token).</param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IQuote>>>
        GetHistoricalQuotesMultiPagesAsAsyncEnumerable<TRequest>(
            this IHistoricalQuotesClient<TRequest> client,
            TRequest request)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        GetHistoricalQuotesMultiPagesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalQuotesClient{TRequest}.ListHistoricalQuotesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalQuotesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical quotes request (with empty next page token).</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<IQuote>>>
        GetHistoricalQuotesMultiPagesAsAsyncEnumerable<TRequest>(
            this IHistoricalQuotesClient<TRequest> client,
            TRequest request,
            CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IQuote> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull().GetHistoricalQuotesAsync, cancellationToken);
}
