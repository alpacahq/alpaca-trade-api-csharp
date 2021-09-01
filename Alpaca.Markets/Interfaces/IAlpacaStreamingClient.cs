using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca streaming API.
    /// </summary>
    [CLSCompliant(false)]
    public interface IAlpacaStreamingClient : IStreamingClient
    {
        /// <summary>
        /// Occurred when new trade update received from stream.
        /// </summary>
        event Action<ITradeUpdate>? OnTradeUpdate;
    }
}
