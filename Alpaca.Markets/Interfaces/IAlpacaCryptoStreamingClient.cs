namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca crypto data streaming API via websockets.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaCryptoStreamingClient : IStreamingDataClient
{
    /// <summary>
    /// Gets the order book updates subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset name.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    public IAlpacaDataSubscription<IOrderBook> GetOrderBookSubscription(
        String symbol);
}
