using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets.Extensions
{
    internal abstract class ClientWithReconnectBase<TClient> : IStreamingClient
        where TClient : IStreamingClient
    {
        private readonly ISet<SocketError> _retrySocketErrorCodes =
            ThrottleParameters.Default.RetrySocketErrorCodes;

        private readonly CancellationTokenSource _cancellationTokenSource = new ();

        private readonly ReconnectionParameters _reconnectionParameters;

        private SpinLock _closeLock = new (false);

        private SpinLock _errorLock = new (false);

        private volatile Int32 _reconnectionAttempts;

        private readonly Random _random = new ();

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

        public event Action<String>? OnWarning
        {
            add => Client.OnWarning += value;
            remove => Client.OnWarning -= value;
        }

        protected virtual ValueTask OnReconnection(
            CancellationToken cancellationToken) =>
            new (); // DO nothing by default for auto-resubscribed clients.

        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private async void handleSocketClosed()
        {
            var lockTaken = false;
            _closeLock.TryEnter(ref lockTaken);

            if (!lockTaken)
            {
                return;
            }

            try
            {
                await handleSocketClosedImpl().ConfigureAwait(false);
            }
            catch (TaskCanceledException) //-V3163 //-V5606
            {
                // Expected one - don't report
            }
            catch (Exception exception)
            {
                handleOnError(exception);
            }
            finally
            {
                _closeLock.Exit(false);
            }
        }

        private async Task handleSocketClosedImpl()
        {
            var isConnectedAndAuthorized = false;
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
                    isConnectedAndAuthorized = true;
                    break;
                }

                await DisconnectAsync(_cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }

            if (isConnectedAndAuthorized &&
                Interlocked.Exchange(ref _reconnectionAttempts, 0) <=
                _reconnectionParameters.MaxReconnectionAttempts)
            {
                await OnReconnection(_cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
            else
            {
                SocketClosed?.Invoke(); // Finally report to clients
            }
        }

        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        private async void handleOnError(
            Exception exception)
        {
            var lockTaken = false;
            _errorLock.TryEnter(ref lockTaken);

            if (!lockTaken)
            {
                return;
            }

            try
            {
                await handleErrorImpl(exception).ConfigureAwait(false);
            }
            catch (TaskCanceledException) //-V3163 //-V5606
            {
                // Expected one - don't report
            }
            catch (Exception innerException)
            {
                Trace.WriteLine(innerException);
            }
            finally
            {
                _errorLock.Exit(false);
            }
        }

        private async Task handleErrorImpl(
            Exception exception)
        {
            switch (exception)
            {
                case SocketException socketException:
                    if (!_retrySocketErrorCodes.Contains(socketException.SocketErrorCode))
                    {
                        OnError?.Invoke(exception);
                    }
                    await DisconnectAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
                    break;

                case RestClientErrorException:
                    OnError?.Invoke(exception);
                    break;

                case TaskCanceledException: // Expected one - don't report
                    break;

                default:
                    OnError?.Invoke(exception);
                    await DisconnectAsync(_cancellationTokenSource.Token).ConfigureAwait(false);
                    break;
            }
        }
    }
}
