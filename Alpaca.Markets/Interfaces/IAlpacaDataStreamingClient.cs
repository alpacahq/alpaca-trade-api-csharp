namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca data streaming API via websockets.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaDataStreamingClient : IStreamingDataClient
{
    /// <summary>
    /// Gets the trading statuses subscription for the <paramref name="symbol"/> asset.
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
    IAlpacaDataSubscription<IStatus> GetStatusSubscription(
        String symbol);

    /// <summary>
    /// Gets the trade cancellations subscription for the <paramref name="symbol"/> asset.
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
    IAlpacaDataSubscription<ITrade> GetCancellationSubscription(
        String symbol);

    /// <summary>
    /// Gets the trade corrections subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset symbol.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<ICorrection> GetCorrectionSubscription(
        String symbol);

    /// <summary>
    /// Gets the LULD (limit up / limit down) subscription for the <paramref name="symbol"/> asset.
    /// </summary>
    /// <param name="symbol">Alpaca asset symbol.</param>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
        String symbol);
}
