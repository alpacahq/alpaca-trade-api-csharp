using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs.
    /// </summary>
    public interface IStreamingClientBase : IDisposable
    {
        /// <summary>
        /// Occured when stream successfully connected.
        /// </summary>
        event Action<AuthStatus>? Connected;

        /// <summary>
        /// Occured when underlying web socket successfully opened.
        /// </summary>
        event Action? SocketOpened;

        /// <summary>
        /// Occured when underlying web socket successfully closed.
        /// </summary>
        event Action? SocketClosed;

        /// <summary>
        /// Occured when any error happened in stream.
        /// </summary>
        event Action<Exception>? OnError;

        /// <summary>
        /// Opens connection to a streaming API.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
        Task ConnectAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Opens connection to a streaming API and awaits for authentication response.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Awaitable task object for handling client authentication event in asynchronous mode.</returns>
        Task<AuthStatus> ConnectAndAuthenticateAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Closes connection to a streaming API.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
        Task DisconnectAsync(
            CancellationToken cancellationToken = default);
    }
}
