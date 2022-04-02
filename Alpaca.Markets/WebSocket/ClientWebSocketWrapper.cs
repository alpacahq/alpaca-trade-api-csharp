using System.Buffers;
using System.Net.WebSockets;

namespace Alpaca.Markets;

internal sealed class ClientWebSocketWrapper : IWebSocket
{
    private readonly ClientWebSocket _client = new ();

    public void Dispose() => _client.Dispose();

    public Task ConnectAsync(
        Uri uri, CancellationToken cancellationToken) =>
        _client.ConnectAsync(uri, cancellationToken);

    public ValueTask SendAsync(
        ReadOnlySequence<Byte> buffer) =>
        _client.SendAsync(buffer, WebSocketMessageType.Binary);

    public ValueTask<ReceiveResult> ReceiveAsync(
        Memory<Byte> buffer)
    {
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
        return _client.ReceiveAsync(buffer, CancellationToken.None).AsResult();
#elif NETSTANDARD2_0 || NETFRAMEWORK
        var _ = System.Runtime.InteropServices.MemoryMarshal.TryGetArray<Byte>(buffer, out var arraySegment);
        return _client.ReceiveAsync(arraySegment, CancellationToken.None).AsResult();
#else
#error TFMs need to be updated
#endif
    }

    public Task CloseOutputAsync(
        WebSocketCloseStatus closeStatus) =>
        _client.CloseOutputAsync(closeStatus, String.Empty, CancellationToken.None);

    public void Abort() => _client.Abort();

    public WebSocketState State => _client.State;

    public WebSocketCloseStatus? CloseStatus => _client.CloseStatus;
}
