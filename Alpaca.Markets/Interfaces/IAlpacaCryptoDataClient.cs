namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca Crypto Data API via HTTP/REST.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaCryptoDataClient :
    IHistoricalQuotesClient<HistoricalCryptoQuotesRequest>,
    IHistoricalTradesClient<HistoricalCryptoTradesRequest>,
    IHistoricalBarsClient<HistoricalCryptoBarsRequest>,
    IDisposable
{
    /// <summary>
    /// Gets most recent bar for a single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only latest bar information.</returns>
    [UsedImplicitly]
    Task<IBar> GetLatestBarAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent bar for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbols list and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest bars information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,IBar>> ListLatestBarsAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent trade for a single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only latest trade information.</returns>
    [UsedImplicitly]
    Task<ITrade> GetLatestTradeAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent trade for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbols list and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest trades information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,ITrade>> ListLatestTradesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current quote for single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current quote information.</returns>
    [UsedImplicitly]
    Task<IQuote> GetLatestQuoteAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent quote for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbols list and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest quotes information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,IQuote>> ListLatestQuotesAsync(
        LatestDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current cross-exchange best bid/offer (XBBO) for single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol and exchanges list pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current XBBO information.</returns>
    [UsedImplicitly]
    Task<IQuote> GetLatestBestBidOfferAsync(
        LatestBestBidOfferRequest request,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets current cross-exchange best bid/offer (XBBO) for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol and exchanges list pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the current XBBO information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, IQuote>> ListLatestBestBidOffersAsync(
        LatestBestBidOfferListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot data for single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current snapshot information.</returns>
    [UsedImplicitly]
    Task<ISnapshot> GetSnapshotAsync(
        SnapshotDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot data for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset symbol and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the current snapshot information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String,ISnapshot>> ListSnapshotsAsync(
        SnapshotDataListRequest request,
        CancellationToken cancellationToken = default);
}
