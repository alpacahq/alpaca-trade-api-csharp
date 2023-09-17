namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca Screener API via HTTP/REST.
/// </summary>
public interface IAlpacaScreenerClient
{
    /// <summary>
    /// Returns top market movers (gainers and losers) and corresponding price change values.
    /// </summary>
    /// <param name="numberOfLosersAndGainersInResponse">
    /// Number of top market movers to fetch (gainers and losers). Will return number top for each. By default 10 gainers and 10 losers.
    /// </param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
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
    /// <returns>Read-only market movers collections.</returns>
    [UsedImplicitly]
    Task<IMarketMovers> GetTopMarketMoversAsync(
        Int32? numberOfLosersAndGainersInResponse = default,
        CancellationToken cancellationToken = default);
}
