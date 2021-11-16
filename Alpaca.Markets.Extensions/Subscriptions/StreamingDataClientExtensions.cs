namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IStreamingDataClient"/> interface.
/// </summary>
public static class StreamingDataClientExtensions
{
    /// <summary>
    /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
    /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static AlpacaValueTask SubscribeAsync(
        this IStreamingDataClient client,
        params IAlpacaDataSubscription[] subscriptions) =>
        new(token => client.SubscribeAsync(subscriptions, token), default);

    /// <summary>
    /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IStreamingDataClient"/> interface.</param>
    /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static AlpacaValueTask UnsubscribeAsync(
        this IStreamingDataClient client,
        params IAlpacaDataSubscription[] subscriptions) =>
        new(token => client.UnsubscribeAsync(subscriptions, token), default);
}
