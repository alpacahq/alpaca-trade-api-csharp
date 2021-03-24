using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    internal abstract class ClientWithReconnectBase<TClient> : IStreamingClient
        where TClient : IStreamingClient
    {
            private readonly CancellationTokenSource _cancellationTokenSource = new ();

            private readonly ReconnectionParameters _reconnectionParameters;

            private readonly Random _random = new ();

            private Int32 _reconnectionAttempts;

            protected readonly TClient Client;

            private SpinLock _lock;

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
                var lockTaken = false;
                _lock.TryEnter(ref lockTaken);

                if (!lockTaken)
                {
                    return;
                }

                try
                {
                    await handleSocketClosedImpl().ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    handleOnError(exception);
                }
                finally
                {
                    _lock.Exit();
                }
            }

            private async Task handleSocketClosedImpl()
            {
                while (!_cancellationTokenSource.IsCancellationRequested &&
                       Interlocked.Increment(ref _reconnectionAttempts) <=
                       _reconnectionParameters.MaxReconnectionAttempts)
                {
#pragma warning disable CA5394 // Do not use insecure randomness
                    await Task.Delay(_random.Next(
#pragma warning restore CA5394 // Do not use insecure randomness
                                (Int32) _reconnectionParameters.MinReconnectionDelay.TotalMilliseconds,
                                (Int32) _reconnectionParameters.MaxReconnectionDelay.TotalMilliseconds),
                            _cancellationTokenSource.Token)
                        .ConfigureAwait(false);

                    var authStatus = await ConnectAndAuthenticateAsync(_cancellationTokenSource.Token)
                        .ConfigureAwait(false);

                    if (authStatus == AuthStatus.Authorized)
                    {
                        break;
                    }

                    await DisconnectAsync(_cancellationTokenSource.Token)
                        .ConfigureAwait(false);
                }

                if (Interlocked.Exchange(ref _reconnectionAttempts, 0) <=
                    _reconnectionParameters.MaxReconnectionAttempts)
                {
                    OnReconnection(_cancellationTokenSource.Token);
                }
                else
                {
                    SocketClosed?.Invoke(); // Finally report to clients
                }
            }

            private void handleOnError(Exception exception) => OnError?.Invoke(exception);
    }
}
