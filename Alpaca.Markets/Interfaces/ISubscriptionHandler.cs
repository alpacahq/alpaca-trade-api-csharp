namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for subscribing/unsubscribing streaming events.
/// </summary>
public interface ISubscriptionHandler
{
    /// <summary>
    /// Subscribes a single <paramref name="subscription"/> object for receiving data from the server.
    /// </summary>
    /// <param name="subscription">Subscription target - asset and update type holder.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscription"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask SubscribeAsync(
        IAlpacaDataSubscription subscription);

    /// <summary>
    /// Subscribes a single <paramref name="subscription"/> object for receiving data from the server.
    /// </summary>
    /// <param name="subscription">Subscription target - asset and update type holder.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscription"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask SubscribeAsync(
        IAlpacaDataSubscription subscription,
        CancellationToken cancellationToken);

    /// <summary>
    /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
    /// </summary>
    /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscriptions"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask SubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions);

    /// <summary>
    /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
    /// </summary>
    /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscriptions"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask SubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions,
        CancellationToken cancellationToken);

    /// <summary>
    /// Unsubscribes the single <paramref name="subscription"/> object for receiving data from the server.
    /// </summary>
    /// <param name="subscription">Subscription target - asset and update type holder.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscription"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask UnsubscribeAsync(
        IAlpacaDataSubscription subscription);

    /// <summary>
    /// Unsubscribes the single <paramref name="subscription"/> object for receiving data from the server.
    /// </summary>
    /// <param name="subscription">Subscription target - asset and update type holder.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscription"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask UnsubscribeAsync(
        IAlpacaDataSubscription subscription,
        CancellationToken cancellationToken);

    /// <summary>
    /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
    /// </summary>
    /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscriptions"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask UnsubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions);

    /// <summary>
    /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
    /// </summary>
    /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="subscriptions"/> argument is <c>null</c>.
    /// </exception>
    [UsedImplicitly]
    ValueTask UnsubscribeAsync(
        IEnumerable<IAlpacaDataSubscription> subscriptions,
        CancellationToken cancellationToken);
}
