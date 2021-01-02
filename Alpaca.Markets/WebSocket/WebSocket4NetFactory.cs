using System;
using System.Security.Authentication;
using System.Threading;
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
#pragma warning disable CA5398 // Avoid hardcoded SslProtocols values
                    sslProtocols: SslProtocols.Tls12)
#pragma warning restore CA5398 // Avoid hardcoded SslProtocols values
                {
                    EnableAutoSendPing = true, 
                    AutoSendPingInterval = 30
                };

                _webSocket.Opened += handleOpened;
                _webSocket.Closed += handleClosed;

                _webSocket.DataReceived += handleDataReceived;
                _webSocket.MessageReceived += handleMessageReceived;

                _webSocket.Error += handleError;
            }

            public void Dispose()
            {
                _webSocket.Opened -= handleOpened;
                _webSocket.Closed -= handleClosed;

                _webSocket.DataReceived -= handleDataReceived;
                _webSocket.MessageReceived -= handleMessageReceived;

                _webSocket.Error -= handleError;

                _webSocket.Dispose();
            }

            public Task OpenAsync(
                CancellationToken cancellationToken) =>
#if NET45
                Task.Run(() => _webSocket.Open(), cancellationToken);
#else
                _webSocket.OpenAsync();
#endif

            public Task CloseAsync(
                CancellationToken cancellationToken) =>
#if NET45
                Task.Run(() => _webSocket.Close(), cancellationToken);
#else
                _webSocket.CloseAsync();
#endif

            public void Send(
                String message) =>
                _webSocket.Send(message);

            public event Action? Opened;

            public event Action? Closed;

            public event Action<Byte[]>? DataReceived;

            public event Action<String>? MessageReceived;

            public event Action<Exception>? Error;

            private void handleOpened(
                Object? sender,
                EventArgs eventArgs) =>
                Opened?.Invoke();

            private void handleClosed(
                Object? sender,
                EventArgs eventArgs) =>
                Closed?.Invoke();

            private void handleDataReceived(
                Object? sender,
                DataReceivedEventArgs eventArgs) =>
                DataReceived?.Invoke(eventArgs.Data);

            private void handleMessageReceived(
                Object? sender,
                MessageReceivedEventArgs eventArgs) =>
                MessageReceived?.Invoke(eventArgs.Message);

            private void handleError(
                Object? sender,
                ErrorEventArgs eventArgs) =>
                Error?.Invoke(eventArgs.Exception);
        }

        public static IWebSocketFactory Instance { get; } = new WebSocket4NetFactory();

        public IWebSocket CreateWebSocket(
            Uri url) =>
            new WebSocketWrapper(url);
    }
}
