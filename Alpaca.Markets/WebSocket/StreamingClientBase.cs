using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides base implementation for the websocket streaming APIs clients.
    /// </summary>
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    internal abstract class StreamingClientBase<TConfiguration> : IStreamingClient 
        where TConfiguration : StreamingClientConfiguration
    {
        private readonly SynchronizationQueue _queue = new ();

        internal readonly TConfiguration Configuration;

        private readonly WebSocketsTransport _webSocket;

        /// <summary>
        /// Creates new instance of <see cref="StreamingClientBase{TConfiguration}"/> object.
        /// </summary>
        /// <param name="configuration"></param>
        private protected StreamingClientBase(
            TConfiguration configuration)
        {
            Configuration = configuration.EnsureNotNull(nameof(configuration));
            Configuration.EnsureIsValid();

            _webSocket = new WebSocketsTransport(
                Configuration.ApiEndpoint, WebSocketMessageType.Binary);

            _webSocket.Opened += OnOpened;
            _webSocket.Closed += OnClosed;

            _webSocket.MessageReceived += OnMessageReceived;
            _webSocket.DataReceived += onDataReceived;

            _webSocket.Error += HandleError;
            _queue.OnError += HandleError;
        }

        /// <inheritdoc />
        public event Action<AuthStatus>? Connected;

        /// <inheritdoc />
        public event Action? SocketOpened;

        /// <inheritdoc />
        public event Action? SocketClosed;

        /// <inheritdoc />
        public event Action<Exception>? OnError;

        /// <inheritdoc />
        public Task ConnectAsync(
            CancellationToken cancellationToken = default)
            => _webSocket.StartAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<AuthStatus> ConnectAndAuthenticateAsync(
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<AuthStatus>(TaskCreationOptions.RunContinuationsAsynchronously);
            Connected += HandleConnected;
            OnError += HandleOnError;

            await ConnectAsync(cancellationToken).ConfigureAwait(false);
            return await tcs.Task.ConfigureAwait(false);

            void HandleConnected(AuthStatus authStatus)
            {
                Connected -= HandleConnected;
                OnError -= HandleOnError;

                tcs.SetResult(authStatus);
            }

            void HandleOnError(Exception exception) =>
                HandleConnected(
                    exception is SocketException { SocketErrorCode: SocketError.IsConnected }
                        ? AuthStatus.Authorized
                        : AuthStatus.Unauthorized);
        }

        /// <inheritdoc />
        public Task DisconnectAsync(
            CancellationToken cancellationToken = default)
            => _webSocket.StopAsync(cancellationToken);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void OnOpened() => SocketOpened?.Invoke();

        protected virtual void OnClosed() => SocketClosed?.Invoke();

        protected virtual void OnMessageReceived(
            String message)
        {
        }

        /// <summary>
        /// Implement <see cref="IDisposable"/> pattern for inheritable classes.
        /// </summary>
        /// <param name="disposing">If <c>true</c> - dispose managed objects.</param>
        protected virtual void Dispose(
            Boolean disposing)
        {
            if (!disposing)
            {
                return;
            }

            _webSocket.Opened -= OnOpened;
            _webSocket.Closed -= OnClosed;

            _webSocket.MessageReceived -= OnMessageReceived;
            _webSocket.DataReceived -= onDataReceived;

            _webSocket.Error -= HandleError;
            _queue.OnError -= OnError;

            _webSocket.Dispose();
            _queue.Dispose();
        }

        /// <summary>
        /// Handles single incoming message. Select handler from generic handlers map
        /// <paramref name="handlers"/> using <paramref name="messageType"/> parameter
        /// as a key and pass <paramref name="message"/> parameter as value into the
        /// selected handler. All exceptions are caught inside this method and reported
        /// to client via standard <see cref="OnError"/> event.
        /// </summary>
        /// <param name="handlers">Message handlers map.</param>
        /// <param name="messageType">Message type for selecting handler from map.</param>
        /// <param name="message">Message data for processing by selected handler.</param>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected void HandleMessage<TKey>(
            IDictionary<TKey, Action<JToken>> handlers,
            TKey messageType,
            JToken message)
            where TKey : class
        {
            try
            {
                if (handlers.EnsureNotNull(nameof(handlers))
                    .TryGetValue(messageType, out var handler))
                {
                    _queue.Enqueue(() => handler(message));
                }
                else
                {
                    HandleError(new InvalidOperationException(
                        $"Unexpected message type '{messageType}' received."));
                }
            }
            catch (Exception exception)
            {
                HandleError(exception);
            }
        }

        /// <summary>
        /// Raises <see cref="Connected"/> event with specified <paramref name="authStatus"/> value.
        /// </summary>
        /// <param name="authStatus">Authentication status (protocol level) of client.</param>
        protected void OnConnected(
            AuthStatus authStatus) =>
            Connected?.Invoke(authStatus);

        protected void HandleError(
            Exception exception)
        {
            if (exception is SocketException { SocketErrorCode: SocketError.IsConnected }) 
            {
                return; // We skip that error because it doesn't matter for us
            }
            OnError?.Invoke(exception);
        }

        /// <summary>
        /// Send object (JSON serializable) as string into the web socket.
        /// </summary>
        /// <param name="value">Object for serializing and sending.</param>
        protected Task SendAsJsonString(
            Object value)
        {
            using var textWriter = new StringWriter();

            var serializer = new JsonSerializer();
            serializer.Serialize(textWriter, value);
            return _webSocket.SendAsync(textWriter.ToString());
        }

        private void onDataReceived(
            Byte[] binaryData) =>
            OnMessageReceived(Encoding.UTF8.GetString(binaryData));
    }
}
