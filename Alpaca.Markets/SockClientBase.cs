using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs.
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    public abstract class SockClientBase : IDisposable
    {
        private readonly IWebSocket _webSocket;

        /// <summary>
        /// Creates new instance of <see cref="SockClientBase"/> object.
        /// </summary>
        /// <param name="endpointUri">URL for websocket endpoint connection.</param>
        /// <param name="webSocketFactory">Factory class for web socket wrapper creation.</param>
        protected SockClientBase(
            UriBuilder endpointUri,
            IWebSocketFactory webSocketFactory)
        {
            _webSocket = webSocketFactory.CreateWebSocket(endpointUri.Uri);

            _webSocket.Opened += OnOpened;
            _webSocket.Closed += OnClosed;

            _webSocket.MessageReceived += OnMessageReceived;
            _webSocket.DataReceived += OnDataReceived;

            _webSocket.Error += HandleError;
        }

        /// <summary>
        /// Occured when any error happened in stream.
        /// </summary>
        public event Action<Exception> OnError;

        /// <summary>
        /// Opens connection to a streaming API.
        /// </summary>
        /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
        public Task ConnectAsync() => _webSocket.OpenAsync();

        /// <summary>
        /// Closes connection to a streaming API.
        /// </summary>
        /// <returns>Awaitable task object for handling action completion in asynchronous mode.</returns>
        public Task DisconnectAsync() => _webSocket.CloseAsync();

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal abstract JsonAuthRequest GetAuthRequest();

        /// <summary>
        /// Handles <see cref="IWebSocket.Opened"/> event.
        /// </summary>
        protected virtual void OnOpened() => SendAsJsonString(GetAuthRequest());

        /// <summary>
        /// Handles <see cref="IWebSocket.Closed"/> event.
        /// </summary>
        protected virtual void OnClosed()
        {
        }

        /// <summary>
        /// Handles <see cref="IWebSocket.MessageReceived"/> event.
        /// </summary>
        /// <param name="message">Incoming string message for processing.</param>
        protected virtual void OnMessageReceived(
            String message)
        {
        }

        /// <summary>
        /// Handles <see cref="IWebSocket.DataReceived"/> event.
        /// </summary>
        /// <param name="binaryData">Incoming binary data for processing.</param>
        protected virtual void OnDataReceived(
            Byte[] binaryData)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(
            Boolean disposing)
        {
            if (!disposing ||
                _webSocket == null)
            {
                return;
            }

            _webSocket.Opened -= OnOpened;
            _webSocket.Closed -= OnClosed;

            _webSocket.MessageReceived -= OnMessageReceived;
            _webSocket.DataReceived -= OnDataReceived;

            _webSocket.Error -= HandleError;

            _webSocket.Dispose();
        }

        /// <summary>
        /// Handles <see cref="IWebSocket.Error"/> event.
        /// </summary>
        /// <param name="exception">Exception for routing into <see cref="OnError"/> event.</param>
        protected void HandleError(
            Exception exception)
        {
            OnError?.Invoke(exception);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void SendAsJsonString(
            Object value)
        {
            using (var textWriter = new StringWriter())
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(textWriter, value);
                _webSocket.Send(textWriter.ToString());
            }
        }
    }
}
