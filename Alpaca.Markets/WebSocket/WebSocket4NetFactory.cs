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

            public WebSocketWrapper(Uri url)
            {
                _webSocket = new WebSocket(url.ToString(),
                    sslProtocols: SslProtocols.Tls11 | SslProtocols.Tls12);

                _webSocket.Opened += onWebSocketOnOpened;
                _webSocket.Closed += onWebSocketOnClosed;

                _webSocket.DataReceived += onWebSocketOnDataReceived;
                _webSocket.Error += onWebSocketOnError;
            }

            public void Dispose()
            {
                if (_webSocket == null)
                {
                    return;
                }

                _webSocket.Opened -= onWebSocketOnOpened;
                _webSocket.Closed -= onWebSocketOnClosed;

                _webSocket.DataReceived -= onWebSocketOnDataReceived;
                _webSocket.Error -= onWebSocketOnError;

                _webSocket?.Dispose();
            }

            public Task OpenAsync()
            {
#if NET45
            return Task.Run(() => _webSocket.Open());
#else
                return _webSocket.OpenAsync();
#endif
            }

            public Task CloseAsync()
            {
#if NET45
            return Task.Run(() => _webSocket.Close());
#else
                return _webSocket.CloseAsync();
#endif
            }

            public void Send(String message)
            {
                _webSocket.Send(message);
            }

            public event Action Opened;

            public event Action Closed;

            public event Action<Byte[]> DataReceived;

            public event Action<Exception> Error;

            private void onWebSocketOnClosed(Object sender, EventArgs eventArgs)
            {
                Closed?.Invoke();
            }

            private void onWebSocketOnOpened(Object sender, EventArgs eventArgs)
            {
                Opened?.Invoke();
            }

            private void onWebSocketOnError(Object sender, ErrorEventArgs eventArgs)
            {
                Error?.Invoke(eventArgs.Exception);
            }

            private void onWebSocketOnDataReceived(Object sender, DataReceivedEventArgs eventArgs)
            {
                DataReceived?.Invoke(eventArgs.Data);
            }
        }

        public IWebSocket CreateWebSocket(Uri url)
        {
            return new WebSocketWrapper(url);
        }
    }
}
