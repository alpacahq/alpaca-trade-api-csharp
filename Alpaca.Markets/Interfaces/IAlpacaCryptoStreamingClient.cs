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
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    public IAlpacaDataSubscription<IOrderBook> GetOrderBookSubscription(
        String symbol);
}
