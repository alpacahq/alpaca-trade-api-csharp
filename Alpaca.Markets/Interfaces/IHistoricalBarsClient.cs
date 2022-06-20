namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca Crypto Data API via HTTP/REST.
/// </summary>
[CLSCompliant(false)]
public interface IHistoricalBarsClient<in TRequest>
    where TRequest : IHistoricalRequest<TRequest, IBar>
{
    /// <summary>
    /// Gets historical bars list for single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Historical bars request parameters.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only list of historical bars for specified asset (with pagination data).</returns>
    [UsedImplicitly]
    Task<IPage<IBar>> ListHistoricalBarsAsync(
        TRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets historical bars dictionary for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Historical bars request parameters.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only dictionary of historical bars for specified assets (with pagination data).</returns>
    [UsedImplicitly]
    Task<IMultiPage<IBar>> GetHistoricalBarsAsync(
        TRequest request,
        CancellationToken cancellationToken = default);
}
