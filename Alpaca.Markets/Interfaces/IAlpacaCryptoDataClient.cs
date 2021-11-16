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
    /// Gets last trade for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset name and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only last trade information.</returns>
    [UsedImplicitly]
    Task<ITrade> GetLatestTradeAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current quote for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset name and exchange pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current quote information.</returns>
    [UsedImplicitly]
    Task<IQuote> GetLatestQuoteAsync(
        LatestDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current cross-exchange best bid/offer (XBBO) for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Asset name and exchanges list pair for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current quote information.</returns>
    [UsedImplicitly]
    Task<IQuote> GetLatestBestBidOfferAsync(
        LatestBestBidOfferRequest request,
        CancellationToken cancellationToken = default);
}
