namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for websocket streaming APIs with data subscriptions.
/// </summary>
[CLSCompliant(false)]
public interface IStreamingDataClient : IStreamingClient, ISubscriptionHandler
{
    /// <summary>
    /// Gets the trade updates subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset name.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<ITrade> GetTradeSubscription(
        String symbol);

    /// <summary>
    /// Gets the quote updates subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset name.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<IQuote> GetQuoteSubscription(
        String symbol);

    /// <summary>
    /// Gets the minute aggregate (bar) subscription for the all assets.
    /// </summary>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<IBar> GetMinuteBarSubscription();

    /// <summary>
    /// Gets the minute aggregate (bar) subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset name.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<IBar> GetMinuteBarSubscription(
        String symbol);

    /// <summary>
    /// Gets the daily aggregate (bar) subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset name.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<IBar> GetDailyBarSubscription(
        String symbol);

    /// <summary>
    /// Gets the updated aggregate (bar) subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset name.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<IBar> GetUpdatedBarSubscription(
        String symbol);
}
