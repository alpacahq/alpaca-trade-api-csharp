using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates generic Alpaca Data streaming API subscription item.
    /// </summary>
    public interface IAlpacaDataSubscription
    {
        /// <summary>
        /// Gets the stream names - updates type (channel name) and asset name (symbol) list.
        /// </summary>
        IEnumerable<String> Streams { get; }

        /// <summary>
        /// Gets boolean flag indicating the current subscription status of this item.
        /// </summary>
        [UsedImplicitly]
        Boolean Subscribed { get; }

        /// <summary>
        /// Occurred when a <see cref="Subscribed"/> property value changed.
        /// </summary>
        [UsedImplicitly]
        event Action? OnSubscribedChanged;
    }

    /// <summary>
    /// Encapsulates strongly-typed Alpaca Data streaming API subscription item.
    /// </summary>
    /// <typeparam name="TApi">Streaming update concrete type.</typeparam>
    public interface IAlpacaDataSubscription<out TApi> : IAlpacaDataSubscription
    {
        /// <summary>
        /// Occurred when a new <typeparamref name="TApi"/> item received from the stream.
        /// </summary>
        event Action<TApi> Received;
    }
}
