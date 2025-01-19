namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca Crypto Data API via HTTP/REST.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaCryptoDataClient :
    IHistoricalQuotesClient<HistoricalCryptoQuotesRequest>,
    IHistoricalTradesClient<HistoricalCryptoTradesRequest>,
    IHistoricalBarsClient<HistoricalCryptoBarsRequest>,
    IAlpacaScreenerClient,
    IRateLimitProvider,
    IDisposable
{
    /// <summary>
    /// Gets most recent bar for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbols list for data retrieval.</param>
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
    /// <returns>Read-only dictionary with the latest bars information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,IBar>> ListLatestBarsAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent trade for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbols list for data retrieval.</param>
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
    /// <returns>Read-only dictionary with the latest trades information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,ITrade>> ListLatestTradesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent quote for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbols list for data retrieval.</param>
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
    /// <returns>Read-only dictionary with the latest quotes information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,IQuote>> ListLatestQuotesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot data for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol for data retrieval.</param>
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
    /// <returns>Read-only dictionary with the current snapshot information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,ISnapshot>> ListSnapshotsAsync(
        SnapshotDataListRequest request,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets current order books for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbols list for data retrieval.</param>
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
    /// <returns>Read-only dictionary with the current order book information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, IOrderBook>> ListLatestOrderBooksAsync(
        LatestOrderBooksRequest request,
        CancellationToken cancellationToken = default);
}
