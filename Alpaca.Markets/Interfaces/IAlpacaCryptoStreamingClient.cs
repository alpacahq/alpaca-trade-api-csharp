using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca data streaming API via websockets.
    /// </summary>
    [CLSCompliant(false)]
    public interface IAlpacaCryptoStreamingClient : IStreamingDataClient
    {
    }
}
