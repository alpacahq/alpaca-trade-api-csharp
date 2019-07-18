using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs.
    /// </summary>
    public abstract class SockClientBase : IDisposable
    {
        internal IWebSocket _webSocket;

        internal void setupWebSocket(UriBuilder endpointUri, IWebSocketFactory webSocketFactory)
        {
            _webSocket = webSocketFactory.CreateWebSocket(endpointUri.Uri);

            _webSocket.Opened += handleOpened;
            _webSocket.Closed += handleClosed;
        }

        internal abstract JsonAuthRequest getAuthRequest();

        private void handleOpened()
        {
            sendAsJsonString(getAuthRequest());
        }

        private void handleClosed()
        {
        }

        /// <summary>
        /// Opens connection to a streaming API.
        /// </summary>
        /// <returns>Waitable task object for handling action completion in asyncronious mode.</returns>
        public Task ConnectAsync()
        {
            return _webSocket.OpenAsync();
        }

        /// <summary>
        /// Closes connection to a streaming API.
        /// </summary>
        /// <returns>Waitable task object for handling action completion in asyncronious mode.</returns>
        public Task DisconnectAsync()
        {
            return _webSocket.CloseAsync();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_webSocket == null)
            {
                return;
            }

            _webSocket.Opened -= handleOpened;
            _webSocket.Closed -= handleClosed;

            _webSocket.Dispose();
        }

        internal void sendAsJsonString(
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
