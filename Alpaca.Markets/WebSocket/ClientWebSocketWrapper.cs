using System.Buffers;
using System.Net.WebSockets;

#if !(NETSTANDARD2_1 || NET6_0_OR_GREATER)
using System.Runtime.InteropServices;
#endif

namespace Alpaca.Markets;

internal sealed class ClientWebSocketWrapper : IWebSocket
{
    private readonly ClientWebSocket _client = new();

    public void Dispose() => _client.Dispose();

    public Task ConnectAsync(
        Uri uri, CancellationToken cancellationToken) =>
        _client.ConnectAsync(uri, cancellationToken);

    public ValueTask SendAsync(
        ReadOnlySequence<Byte> buffer)
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        return buffer.IsSingleSegment
            ? _client.SendAsync(
                buffer.First, WebSocketMessageType.Binary, true, CancellationToken.None)
            : sendMultiSegmentAsync(buffer);

#else
        // ReSharper disable once InvertIf
        if (buffer.IsSingleSegment)
        {
            MemoryMarshal.TryGetArray(buffer.First, out var segment);
            return new ValueTask(_client.SendAsync(
                segment, WebSocketMessageType.Binary, true, CancellationToken.None));
        }

        return sendMultiSegmentAsync(buffer);
#endif
    }

    public ValueTask<ReceiveResult> ReceiveAsync(
        Memory<Byte> buffer)
    {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        return asResult(_client.ReceiveAsync(buffer, CancellationToken.None));
#elif NETSTANDARD2_0 || NETFRAMEWORK
        _ = MemoryMarshal.TryGetArray<Byte>(buffer, out var arraySegment);
        return asResult(_client.ReceiveAsync(arraySegment, CancellationToken.None));
#else
#error TFMs need to be updated
        throw new NotSupportedException();
#endif
    }

    public Task CloseOutputAsync(
        WebSocketCloseStatus closeStatus) =>
        _client.CloseOutputAsync(closeStatus, String.Empty, CancellationToken.None);

    public void Abort() => _client.Abort();

    public WebSocketState State => _client.State;

    public WebSocketCloseStatus? CloseStatus => _client.CloseStatus;

    private async ValueTask sendMultiSegmentAsync(
        ReadOnlySequence<Byte> buffer)
    {
        var position = buffer.Start;
        buffer.TryGet(ref position, out var prevSegment);

        while (buffer.TryGet(ref position, out var segment))
        {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
            await _client.SendAsync(
                prevSegment, WebSocketMessageType.Binary, false, CancellationToken.None).ConfigureAwait(false);
#else
            MemoryMarshal.TryGetArray(prevSegment, out var arraySegment);
            await _client.SendAsync(
                arraySegment, WebSocketMessageType.Binary, false, CancellationToken.None).ConfigureAwait(false);
#endif
            prevSegment = segment;
        }

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        await _client.SendAsync(
            prevSegment, WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(false);
#else
        _ = MemoryMarshal.TryGetArray(prevSegment, out var arraySegmentEnd);
        await _client.SendAsync(
            arraySegmentEnd, WebSocketMessageType.Binary, true, CancellationToken.None).ConfigureAwait(false);
#endif
    }

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
    private static async ValueTask<ReceiveResult> asResult(
        ValueTask<ValueWebSocketReceiveResult> result) =>
        asResult(await result.ConfigureAwait(false));

    private static ReceiveResult asResult(
        ValueWebSocketReceiveResult result) =>
        new(result.MessageType, result.EndOfMessage, result.Count);
#elif NETSTANDARD2_0 || NETFRAMEWORK
    private static async ValueTask<ReceiveResult> asResult(
        Task<WebSocketReceiveResult> result) =>
        asResult(await result.ConfigureAwait(false));

    private static ReceiveResult asResult(
        WebSocketReceiveResult result) =>
        new(result.MessageType, result.EndOfMessage, result.Count);
#endif
}
