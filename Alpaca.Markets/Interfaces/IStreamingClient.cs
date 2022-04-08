namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for websocket streaming APIs.
/// </summary>
public interface IStreamingClient : IDisposable
{
    /// <summary>
    /// Invoked when stream is successfully connected.
    /// </summary>
    [UsedImplicitly]
    event Action<AuthStatus>? Connected;

    /// <summary>
    /// Invoked when underlying web socket is successfully opened.
    /// </summary>
    [UsedImplicitly]
    event Action? SocketOpened;

    /// <summary>
    /// Invoked when underlying web socket is successfully closed.
    /// </summary>
    [UsedImplicitly]
    event Action? SocketClosed;

    /// <summary>
    /// Invoked when any error occurs in stream.
    /// </summary>
    [UsedImplicitly]
    event Action<Exception>? OnError;

    /// <summary>
    /// Invoked in case of non-critical events.
    /// </summary>
    [UsedImplicitly]
    event Action<String>? OnWarning;

    /// <summary>
    /// Opens connection to a streaming API.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
    [UsedImplicitly]
    Task ConnectAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Opens connection to a streaming API and awaits for authentication response.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Awaitable task object for handling client authentication event in asynchronous mode.</returns>
    [UsedImplicitly]
    Task<AuthStatus> ConnectAndAuthenticateAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes connection to a streaming API.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
    [UsedImplicitly]
    Task DisconnectAsync(
        CancellationToken cancellationToken = default);
}
