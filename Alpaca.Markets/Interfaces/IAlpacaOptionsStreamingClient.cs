namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca options data streaming API via websockets.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaOptionsStreamingClient : IStreamingDataClient
{
    // Note: Options streaming inherits all standard streaming capabilities from IStreamingDataClient
    // including GetTradeSubscription(), GetQuoteSubscription(), and bar subscriptions.
    // The WebSocket endpoint supports both 'trades' and 'quotes' channels for options data.
    // Options bars are not available in the streaming API based on Alpaca documentation.
}