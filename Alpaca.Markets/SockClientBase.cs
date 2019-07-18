using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using System.Security.Authentication;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for websocket streaming APIs.
    /// </summary>
    public abstract partial class SockClientBase : IDisposable
    {
        private protected WebSocket _webSocket;

        private protected void setupWebSocket(String endpointUri)
        {
            _webSocket = new WebSocket(endpointUri,
                sslProtocols: SslProtocols.Tls11 | SslProtocols.Tls12);

            _webSocket.Opened += handleOpened;
            _webSocket.Closed += handleClosed;
        }

        private protected abstract JsonAuthRequest getAuthRequest();

        private void handleOpened(
            Object sender,
            EventArgs e)
        {
            sendAsJsonString(getAuthRequest());
        }

        private void handleClosed(
            Object sender,
            EventArgs e)
        {
        }


        /// <summary>
        /// Opens connection to a streaming API.
        /// </summary>
        /// <returns>Waitable task object for handling action completion in asyncronious mode.</returns>
        public Task ConnectAsync()
        {
#if NET45
            return Task.Run(() => _webSocket.Open());
#else
            return _webSocket.OpenAsync();
#endif
        }

        /// <summary>
        /// Closes connection to a streaming API.
        /// </summary>
        /// <returns>Waitable task object for handling action completion in asyncronious mode.</returns>
        public Task DisconnectAsync()
        {
#if NET45
            return Task.Run(() => _webSocket.Close());
#else
            return _webSocket.CloseAsync();
#endif
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _webSocket?.Dispose();
        }

        private protected void sendAsJsonString(
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
