using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    internal abstract class ClientWithReconnectBase<TClient> : IStreamingClient
        where TClient : IStreamingClient
    {
            private readonly CancellationTokenSource _cancellationTokenSource =
                new CancellationTokenSource();

            private readonly ReconnectionParameters _reconnectionParameters;

            private readonly Random _random = new Random();

            protected readonly TClient Client;

            protected ClientWithReconnectBase(
                TClient client,
                ReconnectionParameters reconnectionParameters)
            {
                Client = client;
                _reconnectionParameters = reconnectionParameters;
                Client.SocketClosed += handleSocketClosed;
                Client.OnError += handleOnError;
            }

            public void Dispose()
            {
                Client.SocketClosed -= handleSocketClosed;
                _cancellationTokenSource.Cancel();

                Client.Dispose();
                _cancellationTokenSource.Dispose();
            }

            public Task ConnectAsync(
                CancellationToken cancellationToken = default) =>
                Client.ConnectAsync(cancellationToken);

            public Task<AuthStatus> ConnectAndAuthenticateAsync(
                CancellationToken cancellationToken = default) =>
                Client.ConnectAndAuthenticateAsync(cancellationToken);

            public Task DisconnectAsync(
                CancellationToken cancellationToken = default)
            {
                Client.SocketClosed -= handleSocketClosed;
                _cancellationTokenSource.Cancel();

                return Client.DisconnectAsync(cancellationToken);
            }

            public event Action<AuthStatus>? Connected
            {
                add => Client.Connected += value;
                remove => Client.Connected -= value;
            }

            public event Action? SocketOpened
            {
                add => Client.SocketOpened += value;
                remove => Client.SocketOpened -= value;
            }

            public event Action? SocketClosed;

            public event Action<Exception>? OnError;

            protected virtual void OnReconnection(
                CancellationToken cancellationToken)
            {
                // DO nothing by default for auto-resubscribed clients.
            }

            [SuppressMessage(
                "Design", "CA1031:Do not catch general exception types",
                Justification = "Expected behavior - we report exceptions via OnError event.")]
            private async void handleSocketClosed()
            {
                try
                {
                    var reconnectionAttempts = 0;

                    while (!_cancellationTokenSource.IsCancellationRequested &&
                           reconnectionAttempts < _reconnectionParameters.MaxReconnectionAttempts)
                    {
#pragma warning disable CA5394 // Do not use insecure randomness
                        await Task.Delay(_random.Next(
#pragma warning restore CA5394 // Do not use insecure randomness
                                    (Int32)_reconnectionParameters.MinReconnectionDelay.TotalMilliseconds, 
                                    (Int32)_reconnectionParameters.MaxReconnectionDelay.TotalMilliseconds), 
                                _cancellationTokenSource.Token)
                            .ConfigureAwait(false);

                        var authStatus = await ConnectAndAuthenticateAsync(_cancellationTokenSource.Token)
                            .ConfigureAwait(false);

                        if (authStatus == AuthStatus.Authorized)
                        {
                            OnReconnection(_cancellationTokenSource.Token);
                            return; // Reconnected, authorized and re-subscribed
                        }

                        ++reconnectionAttempts;
                    }

                    SocketClosed?.Invoke(); // Finally report to clients
                }
                catch (Exception exception)
                {
                    handleOnError(exception);
                }
            }

            private void handleOnError(Exception exception) => OnError?.Invoke(exception);
    }
}
