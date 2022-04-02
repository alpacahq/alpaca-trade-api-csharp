using System.Net.WebSockets;

namespace Alpaca.Markets;

internal static class ReceiveResultExtensions
{
#if NETSTANDARD2_1 || NET5_0_OR_GREATER
    internal static async ValueTask<ReceiveResult> AsResult(
        this ValueTask<ValueWebSocketReceiveResult> result) =>
        asResult(await result.ConfigureAwait(false));

    private static ReceiveResult asResult(
        ValueWebSocketReceiveResult result) =>
        new (result.MessageType, result.EndOfMessage, result.Count);
#elif NETSTANDARD2_0 || NETFRAMEWORK
    internal static async ValueTask<ReceiveResult> AsResult(
        this Task<WebSocketReceiveResult> result) =>
        asResult(await result.ConfigureAwait(false));

    private static ReceiveResult asResult(
        WebSocketReceiveResult result) =>
        new (result.MessageType, result.EndOfMessage, result.Count);
#endif
}
