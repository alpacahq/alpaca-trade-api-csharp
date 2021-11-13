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
    /// <returns>Read-only dictionary of historical bars for specified assets (with pagination data).</returns>
    [UsedImplicitly]
    Task<IMultiPage<IBar>> GetHistoricalBarsAsync(
        TRequest request,
        CancellationToken cancellationToken = default);
}
