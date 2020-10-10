using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    internal abstract class ClientWithReconnectBase<TClient, TSubscription> : IStreamingClientBase
        where TClient : IStreamingClientBase
    {
            protected readonly ConcurrentDictionary<String, TSubscription> _subscriptions =
                new ConcurrentDictionary<String, TSubscription>(StringComparer.Ordinal);

            private readonly CancellationTokenSource _cancellationTokenSource =
                new CancellationTokenSource();

            private readonly ReconnectionParameters _reconnectionParameters;

            private readonly Random _random = new Random();

            protected readonly TClient _client;

            protected ClientWithReconnectBase(
                TClient client,
                ReconnectionParameters reconnectionParameters)
            {
                _client = client;
                _reconnectionParameters = reconnectionParameters;
                _client.SocketClosed += handleSocketClosed;
            }

            public void Dispose()
            {
                _client.SocketClosed -= handleSocketClosed;
                _cancellationTokenSource.Cancel();

                _client.Dispose();
                _cancellationTokenSource.Dispose();
            }

            public Task ConnectAsync(
                CancellationToken cancellationToken = default) =>
                _client.ConnectAsync(cancellationToken);

            public Task<AuthStatus> ConnectAndAuthenticateAsync(
                CancellationToken cancellationToken = default) =>
                _client.ConnectAndAuthenticateAsync(cancellationToken);

            public Task DisconnectAsync(
                CancellationToken cancellationToken = default)
            {
                _client.SocketClosed -= handleSocketClosed;
                _cancellationTokenSource.Cancel();

                return _client.DisconnectAsync(cancellationToken);
            }

            public event Action<AuthStatus>? Connected
            {
                add => _client.Connected += value;
                remove => _client.Connected -= value;
            }

            public event Action? SocketOpened
            {
                add => _client.SocketOpened += value;
                remove => _client.SocketOpened -= value;
            }

            public event Action? SocketClosed
            {
                add => _client.SocketClosed += value;
                remove => _client.SocketClosed -= value;
            }

            public event Action<Exception>? OnError
            {
                add => _client.OnError += value;
                remove => _client.OnError -= value;
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
                        foreach (var kvp in _subscriptions.ToArray())
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
