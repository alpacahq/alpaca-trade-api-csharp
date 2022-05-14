namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca Crypto Data API via HTTP/REST.
/// </summary>
[CLSCompliant(false)]
public interface IHistoricalTradesClient<in TRequest>
    where TRequest : IHistoricalRequest<TRequest, ITrade>
{

    /// <summary>
    /// Gets historical trades list for single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Historical trades request parameters.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only list of historical trades for specified asset (with pagination data).</returns>
    [UsedImplicitly]
    Task<IPage<ITrade>> ListHistoricalTradesAsync(
        TRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets historical trades dictionary for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Historical trades request parameters.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary of historical trades for specified assets (with pagination data).</returns>
    [UsedImplicitly]
    Task<IMultiPage<ITrade>> GetHistoricalTradesAsync(
        TRequest request,
        CancellationToken cancellationToken = default);
}
