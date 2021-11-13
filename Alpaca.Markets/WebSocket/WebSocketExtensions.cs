using System.Buffers;
using System.Net.WebSockets;

#if !(NETSTANDARD2_1 || NET5_0_OR_GREATER)
using System.Runtime.InteropServices;
#endif

namespace Alpaca.Markets;

internal static class WebSocketExtensions
{
    public static ValueTask SendAsync(
        this WebSocket webSocket,
        ReadOnlySequence<byte> buffer,
        WebSocketMessageType webSocketMessageType,
        CancellationToken cancellationToken = default)
    {
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
            return buffer.IsSingleSegment
                ? webSocket.SendAsync(buffer.First, webSocketMessageType, true, cancellationToken)
                : sendMultiSegmentAsync(webSocket, buffer, webSocketMessageType, cancellationToken);

#else
        // ReSharper disable once InvertIf
        if (buffer.IsSingleSegment)
        {
            MemoryMarshal.TryGetArray(buffer.First, out var segment);
            return new ValueTask(webSocket.SendAsync(segment, webSocketMessageType, true, cancellationToken));
        }

        return sendMultiSegmentAsync(webSocket, buffer, webSocketMessageType, cancellationToken);
#endif
    }

    private static async ValueTask sendMultiSegmentAsync(
        WebSocket webSocket,
        ReadOnlySequence<byte> buffer,
        WebSocketMessageType webSocketMessageType,
        CancellationToken cancellationToken = default)
    {
        var position = buffer.Start;
        // Get a segment before the loop so we can be one segment behind while writing
        // This allows us to do a non-zero byte write for the endOfMessage = true send
        buffer.TryGet(ref position, out var prevSegment);
        while (buffer.TryGet(ref position, out var segment))
        {
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
                await webSocket.SendAsync(prevSegment, webSocketMessageType, false, cancellationToken).ConfigureAwait(false);
#else
            MemoryMarshal.TryGetArray(prevSegment, out var arraySegment);
            await webSocket.SendAsync(arraySegment, webSocketMessageType, false, cancellationToken).ConfigureAwait(false);
#endif
            prevSegment = segment;
        }

        // End of message frame
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
            await webSocket.SendAsync(prevSegment, webSocketMessageType, true, cancellationToken).ConfigureAwait(false);
#else
        _ = MemoryMarshal.TryGetArray(prevSegment, out var arraySegmentEnd);
        await webSocket.SendAsync(arraySegmentEnd, webSocketMessageType, true, cancellationToken).ConfigureAwait(false);
#endif
    }
}
