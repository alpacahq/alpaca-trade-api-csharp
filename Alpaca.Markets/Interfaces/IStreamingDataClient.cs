using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
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
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        ValueTask SubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Subscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        ValueTask SubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes the single <paramref name="subscription"/> object for receiving data from the server.
        /// </summary>
        /// <param name="subscription">Subscription target - asset and update type holder.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        ValueTask UnsubscribeAsync(
            IAlpacaDataSubscription subscription,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Unsubscribes several <paramref name="subscriptions"/> objects for receiving data from the server.
        /// </summary>
        /// <param name="subscriptions">List of subscription targets - assets and update type holders.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        ValueTask UnsubscribeAsync(
            IEnumerable<IAlpacaDataSubscription> subscriptions,
            CancellationToken cancellationToken = default);
    }
}
