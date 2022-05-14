using System.Net.WebSockets;

namespace Alpaca.Markets;

/// <summary>
/// Helper struct for holding results of <see cref="IWebSocket.ReceiveAsync"/> method calls.
/// </summary>
/// <param name="MessageType">Web socket message type (text, binary, close).</param>
/// <param name="EndOfMessage">Is <c>true</c> for the last frame in message.</param>
/// <param name="Count">Number of bytes in the last frame.</param>
[ExcludeFromCodeCoverage]
public readonly record struct ReceiveResult(
    WebSocketMessageType MessageType,
    Boolean EndOfMessage,
    Int32 Count);
