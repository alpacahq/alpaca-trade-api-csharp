using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    internal abstract class ClientWithReconnectBase<TClient, TSubscription> : IStreamingClientBase
        where TClient : IStreamingClientBase
    {
            protected readonly ConcurrentDictionary<String, TSubscription> Subscriptions =
                new ConcurrentDictionary<String, TSubscription>(StringComparer.Ordinal);

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

            public event Action? SocketClosed
            {
                add => Client.SocketClosed += value;
                remove => Client.SocketClosed -= value;
            }

            public event Action<Exception>? OnError
            {
                add => Client.OnError += value;
                remove => Client.OnError -= value;
            }

            protected abstract void Resubscribe(String symbol, TSubscription subscription);

            private async void handleSocketClosed()
            {
                var reconnectionAttempts = 0;

                while (!_cancellationTokenSource.IsCancellationRequested &&
                       reconnectionAttempts < _reconnectionParameters.MaxReconnectionAttempts)
                {
                    await Task.Delay(_random.Next(
                                (Int32)_reconnectionParameters.MinReconnectionDelay.TotalMilliseconds, 
                                (Int32)_reconnectionParameters.MaxReconnectionDelay.TotalMilliseconds), 
                            _cancellationTokenSource.Token)
                        .ConfigureAwait(false);

                    var authStatus = await ConnectAndAuthenticateAsync(_cancellationTokenSource.Token)
                        .ConfigureAwait(false);

                    if (authStatus == AuthStatus.Authorized)
                    {
                        foreach (var kvp in Subscriptions.ToArray())
                        {
                            if (_cancellationTokenSource.IsCancellationRequested)
                            {
                                return;
                            }
                            Resubscribe(kvp.Key, kvp.Value);
                        }
                    }

                    ++reconnectionAttempts;
                }
            }
    }
}
