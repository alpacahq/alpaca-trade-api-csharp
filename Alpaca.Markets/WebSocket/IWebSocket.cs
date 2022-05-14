using System.Buffers;
using System.Net.WebSockets;

namespace Alpaca.Markets;

/// <summary>
/// Encapsulates asynchronous interface of web socket client.
/// </summary>
public interface IWebSocket : IDisposable
{
    /// <summary>
    /// Connects specified endpoint using web socket protocol.
    /// </summary>
    /// <param name="uri">The web socket endpoint URL for connection.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    Task ConnectAsync(
        Uri uri,
        CancellationToken cancellationToken);

    /// <summary>
    /// Sends message into the web socket from memory buffer.
    /// </summary>
    /// <param name="buffer">Memory buffer with binary message.</param>
    ValueTask SendAsync(
        ReadOnlySequence<Byte> buffer);

    /// <summary>
    /// Reads message frame from the web socket into memory.
    /// </summary>
    /// <param name="buffer">Memory buffer for receiving data.</param>
    /// <returns>Read action status (frame parameters).</returns>
    ValueTask<ReceiveResult> ReceiveAsync(
        Memory<Byte> buffer);

    /// <summary>
    /// Disconnects web socket channel from endpoint.
    /// </summary>
    /// <param name="closeStatus">Disconnection code.</param>
    Task CloseOutputAsync(
        WebSocketCloseStatus closeStatus);

    /// <summary>
    /// Immediately aborts connection at socket level.
    /// </summary>
    void Abort();

    /// <summary>
    /// Gets current web socket channel state.
    /// </summary>
    WebSocketState State { get; }

    /// <summary>
    /// Gets current web socket channel close status (if any).
    /// </summary>
    WebSocketCloseStatus? CloseStatus { get; }
}
