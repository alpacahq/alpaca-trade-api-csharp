using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs with data subscriptions.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IStreamingDataClient : IStreamingClient
    {
        /// <summary>
        /// Subscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        ValueTask SubscribeAsync(
            IAlpacaDataSubscription subscription);

        /// <summary>
        /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        ValueTask SubscribeAsync(
            params IAlpacaDataSubscription[] subscriptions);

        /// <summary>
        /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        ValueTask SubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions);

        /// <summary>
        /// Unsubscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        ValueTask UnsubscribeAsync(
            IAlpacaDataSubscription subscription);

        /// <summary>
        /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        ValueTask UnsubscribeAsync(
            params IAlpacaDataSubscription[] subscriptions);

        /// <summary>
        /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        ValueTask UnsubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions);
    }
}
