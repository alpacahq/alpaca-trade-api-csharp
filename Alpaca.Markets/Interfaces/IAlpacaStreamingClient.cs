using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca streaming API.
    /// </summary>
    public interface IAlpacaStreamingClient : IStreamingClientBase
    {
        /// <summary>
        /// Occurred when new account update received from stream.
        /// </summary>
        [UsedImplicitly]
        [Obsolete("This event never raised and will be removed in the next major SDK release.", true)]
        event Action<IAccountUpdate>? OnAccountUpdate;

        /// <summary>
        /// Occurred when new trade update received from stream.
        /// </summary>
        event Action<ITradeUpdate>? OnTradeUpdate;
    }
}
