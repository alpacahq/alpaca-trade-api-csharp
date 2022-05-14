namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca streaming API.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaStreamingClient : IStreamingClient
{
    /// <summary>
    /// Occurs when a new trade update is received from the stream.
    /// </summary>
    [UsedImplicitly]
    event Action<ITradeUpdate>? OnTradeUpdate;
}
