using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs.
    /// </summary>
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    public abstract class StreamingClientBase<TConfiguration> : IStreamingClientBase 
        where TConfiguration : StreamingClientConfiguration
    {
        private readonly SynchronizationQueue _queue = new SynchronizationQueue();

        private readonly IWebSocket _webSocket;

        internal readonly TConfiguration Configuration;

        /// <summary>
        /// Creates new instance of <see cref="StreamingClientBase{TConfiguration}"/> object.
        /// </summary>
        /// <param name="configuration"></param>
        private protected StreamingClientBase(
            TConfiguration configuration)
        {
            Configuration = configuration.EnsureNotNull(nameof(configuration));
            Configuration.EnsureIsValid();

            _webSocket = configuration.CreateWebSocket();

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
            => _webSocket.OpenAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<AuthStatus> ConnectAndAuthenticateAsync(
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<AuthStatus>();
            Connected += HandleConnected;

            await ConnectAsync(cancellationToken).ConfigureAwait(false);
            return await tcs.Task.ConfigureAwait(false);

            void HandleConnected(AuthStatus authStatus)
            {
                Connected -= HandleConnected;
                tcs.SetResult(authStatus);
            }
        }

        /// <inheritdoc />
        public Task DisconnectAsync(
            CancellationToken cancellationToken = default)
            => _webSocket.CloseAsync(cancellationToken);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles <see cref="IWebSocket.Opened"/> event.
        /// </summary>
        protected virtual void OnOpened() => SocketOpened?.Invoke();

        /// <summary>
        /// Handles <see cref="IWebSocket.Closed"/> event.
        /// </summary>
        protected virtual void OnClosed() => SocketClosed?.Invoke();

        /// <summary>
        /// Handles <see cref="IWebSocket.MessageReceived"/> event.
        /// </summary>
        /// <param name="message">Incoming string message for processing.</param>
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

        /// <summary>
        /// Handles <see cref="IWebSocket.Error"/> event.
        /// </summary>
        /// <param name="exception">Exception for routing into <see cref="OnError"/> event.</param>
        protected void HandleError(
            Exception exception) =>
            OnError?.Invoke(exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void SendAsJsonString(
            Object value)
        {
            using var textWriter = new StringWriter();

            var serializer = new JsonSerializer();
            serializer.Serialize(textWriter, value);
            _webSocket.Send(textWriter.ToString());
        }

        private void onDataReceived(
            Byte[] binaryData) =>
            OnMessageReceived(Encoding.UTF8.GetString(binaryData));
    }
}
