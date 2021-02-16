using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
        Boolean Subscribed { get; }
    }

    /// <summary>
    /// Encapsulates strongly-typed Alpaca Data streaming API subscription item.
    /// </summary>
    /// <typeparam name="TApi">Streaming update concrete type.</typeparam>
    public interface IAlpacaDataSubscription<out TApi> : IAlpacaDataSubscription
        where TApi : IStreamBase
    {
        /// <summary>
        /// Occurred when a new <typeparamref name="TApi"/> item received from the stream.
        /// </summary>
        event Action<TApi> Received;
    }
}
