namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca data streaming API via websockets.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaNewsStreamingClient : IStreamingClient, ISubscriptionHandler
{
    /// <summary>
    /// Gets the news articles' updates subscription for all stock and crypto assets.
    /// </summary>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<INewsArticle> GetNewsSubscription();

    /// <summary>
    /// Gets the news articles' updates subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset symbol.</param>
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
    IAlpacaDataSubscription<INewsArticle> GetNewsSubscription(
        String symbol);
}
