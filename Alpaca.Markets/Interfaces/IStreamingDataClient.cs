using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs with data subscriptions.
    /// </summary>
    public interface IStreamingDataClient : IStreamingClient
    {
        /// <summary>
        /// Subscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask SubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask SubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask UnsubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        [UsedImplicitly]
        ValueTask UnsubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default);
    }
}
