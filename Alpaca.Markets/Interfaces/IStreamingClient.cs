using System.Net.WebSockets;

namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for websocket streaming APIs.
/// </summary>
public interface IStreamingClient : IDisposable
{
    /// <summary>
    /// Occurs when stream is successfully connected.
    /// </summary>
    [UsedImplicitly]
    event Action<AuthStatus>? Connected;

    /// <summary>
    /// Occurs when underlying web socket is successfully opened.
    /// </summary>
    [UsedImplicitly]
    event Action? SocketOpened;

    /// <summary>
    /// Occurs when underlying web socket is successfully closed.
    /// </summary>
    [UsedImplicitly]
    event Action? SocketClosed;

    /// <summary>
    /// Occurs when any error occurs in stream.
    /// </summary>
    [UsedImplicitly]
    event Action<Exception>? OnError;

    /// <summary>
    /// Occurs in case of non-critical events.
    /// </summary>
    [UsedImplicitly]
    event Action<String>? OnWarning;

    /// <summary>
    /// Opens connection to a streaming API.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="SocketException">
    /// The underlying TPC socket connection failed due to an low-level network connectivity issue.
    /// </exception>
    /// <exception cref="WebSocketException">
    /// The WebSocket connection failed due to an high-level protocol or connection issue.
    /// </exception>
    /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
    [UsedImplicitly]
    Task ConnectAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens connection to a streaming API and awaits for authentication response.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="SocketException">
    /// The underlying TPC socket connection failed due to an low-level network connectivity issue.
    /// </exception>
    /// <exception cref="WebSocketException">
    /// The WebSocket connection failed due to an high-level protocol or connection issue.
    /// </exception>
    /// <returns>Awaitable task object for handling client authentication event in asynchronous mode.</returns>
    [UsedImplicitly]
    Task<AuthStatus> ConnectAndAuthenticateAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes connection to a streaming API.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="SocketException">
    /// The underlying TPC socket connection failed due to an low-level network connectivity issue.
    /// </exception>
    /// <exception cref="WebSocketException">
    /// The WebSocket connection failed due to an high-level protocol or connection issue.
    /// </exception>
    /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
    [UsedImplicitly]
    Task DisconnectAsync(
        CancellationToken cancellationToken = default);
}
