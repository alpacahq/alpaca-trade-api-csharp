using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace Alpaca.Markets
{
    internal sealed class WebSocket4NetFactory : IWebSocketFactory
    {
        private sealed class WebSocketWrapper : IWebSocket
        {
            private readonly WebSocket _webSocket;

            public WebSocketWrapper(
                Uri url)
            {
                _webSocket = new WebSocket(url.ToString(),
                    sslProtocols: SslProtocols.Tls11 | SslProtocols.Tls12);

                _webSocket.Opened += handleOpened;
                _webSocket.Closed += handleClosed;

                _webSocket.DataReceived += handleDataReceived;
                _webSocket.MessageReceived += handleMessageReceived;

                _webSocket.Error += handleError;
            }

            public void Dispose()
            {
                if (_webSocket == null)
                {
                    return;
                }

                _webSocket.Opened -= handleOpened;
                _webSocket.Closed -= handleClosed;

                _webSocket.DataReceived -= handleDataReceived;
                _webSocket.MessageReceived -= handleMessageReceived;

                _webSocket.Error -= handleError;

                _webSocket.Dispose();
            }

            public Task OpenAsync() =>
#if NET45
                Task.Run(() => _webSocket.Open());
#else
                _webSocket.OpenAsync();
#endif

            public Task CloseAsync() =>
#if NET45
                Task.Run(() => _webSocket.Close());
#else
                _webSocket.CloseAsync();
#endif

            public void Send(
                String message) =>
                _webSocket.Send(message);

            public event Action Opened;

            public event Action Closed;

            public event Action<Byte[]> DataReceived;

            public event Action<String> MessageReceived;

            public event Action<Exception> Error;

            private void handleOpened(
                Object sender,
                EventArgs eventArgs) =>
                Opened?.Invoke();

            private void handleClosed(
                Object sender,
                EventArgs eventArgs) =>
                Closed?.Invoke();

            private void handleDataReceived(
                Object sender,
                DataReceivedEventArgs eventArgs) =>
                DataReceived?.Invoke(eventArgs.Data);

            private void handleMessageReceived(
                Object sender,
                MessageReceivedEventArgs eventArgs) =>
                MessageReceived?.Invoke(eventArgs.Message);

            private void handleError(
                Object sender,
                ErrorEventArgs eventArgs) =>
                Error?.Invoke(eventArgs.Exception);
        }

        public IWebSocket CreateWebSocket(
            Uri url) =>
            new WebSocketWrapper(url);
    }
}
