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
    public static IAsyncEnumerable<ITrade> GetHistoricalTradesAsAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull().ListHistoricalTradesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.GetHistoricalTradesAsync"/> in pagination
    /// mode as single stream of items (in form of dictionary of the <see cref="IAsyncEnumerable{ITrade}"/>
    /// interface instances) so they  can be consumed by the <c>await foreach</c> statement on the caller side.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
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
    public static IReadOnlyDictionary<String, IAsyncEnumerable<ITrade>> GetHistoricalTradesDictionaryOfAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByItems(client.EnsureNotNull().GetHistoricalTradesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
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
    public static IAsyncEnumerable<IReadOnlyList<ITrade>> GetHistoricalTradesPagesAsAsyncEnumerable<TRequest>(
        this IHistoricalTradesClient<TRequest> client,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull().ListHistoricalTradesAsync, cancellationToken);

    /// <summary>
    /// Gets all items provided by <see cref="IHistoricalTradesClient{TRequest}.ListHistoricalTradesAsync"/> in pagination
    /// mode as single stream of response pages with items so they can be consumed by the <c>await foreach</c>
    /// statement on the caller side as sequence of 'batches' instead of sequence of items itself.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IHistoricalTradesClient{TRequest}"/> interface.</param>
    /// <param name="request">Original historical trades request (with empty next page token).</param>
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
    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<ITrade>>>
        GetHistoricalTradesMultiPagesAsAsyncEnumerable<TRequest>(
            this IHistoricalTradesClient<TRequest> client,
            TRequest request,
            CancellationToken cancellationToken)
        where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, ITrade> =>
        request.EnsureNotNull().GetValidatedRequestWithoutPageToken()
            .GetResponsesByPages(client.EnsureNotNull().GetHistoricalTradesAsync, cancellationToken);
}
