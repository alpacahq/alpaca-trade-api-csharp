using System;

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
        event Action<IAccountUpdate>? OnAccountUpdate;

        /// <summary>
        /// Occurred when new trade update received from stream.
        /// </summary>
        event Action<ITradeUpdate>? OnTradeUpdate;
    }
}
