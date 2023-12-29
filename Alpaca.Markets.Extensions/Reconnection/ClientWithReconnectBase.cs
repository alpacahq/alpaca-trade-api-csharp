using System.Net.WebSockets;

namespace Alpaca.Markets.Extensions;

internal abstract class ClientWithReconnectBase<TClient> : IStreamingClient
    where TClient : class, IStreamingClient
{
    private readonly ISet<SocketError> _retrySocketErrorCodes =
        ThrottleParameters.Default.RetrySocketErrorCodes;

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private readonly ReconnectionParameters _reconnectionParameters;

    private SpinLock _closeLock = new(false);

    private SpinLock _errorLock = new(false);

    private volatile Int32 _reconnectionAttempts;

    private readonly Random _random = new();

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
        Client.OnError -= handleOnError;
        Client.SocketClosed -= handleSocketClosed;

        _cancellationTokenSource.Cancel();

        Client.Dispose();
        _cancellationTokenSource.Dispose();
    }

    public Task ConnectAsync(
        CancellationToken cancellationToken = default) =>
        runWithReconnection(async () =>
        {
            await Client.ConnectAsync(cancellationToken).ConfigureAwait(false);
            return AuthStatus.Unauthorized;
        });

    public Task<AuthStatus> ConnectAndAuthenticateAsync(
        CancellationToken cancellationToken = default) =>
        runWithReconnection(() => Client.ConnectAndAuthenticateAsync(cancellationToken));

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

    public event Action<String>? OnWarning
    {
        add => Client.OnWarning += value;
        remove => Client.OnWarning -= value;
    }

    public event Action? SocketClosed;

    public event Action<Exception>? OnError;

    protected virtual ValueTask OnReconnection(
        CancellationToken cancellationToken) =>
        new(); // DO nothing by default for auto-resubscribed clients.

    private async void handleSocketClosed() => 
        await handleSocketClosedAsync().ConfigureAwait(false);

    [SuppressMessage(
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    private async Task<AuthStatus> handleSocketClosedAsync()
    {
        var lockTaken = false;
        _closeLock.TryEnter(ref lockTaken);

        if (!lockTaken)
        {
            return AuthStatus.Unauthorized;
        }

        try
        {
            return await handleSocketClosedImplAsync().ConfigureAwait(false);
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

        return AuthStatus.Unauthorized;
    }

    private async Task<AuthStatus> handleSocketClosedImplAsync()
    {
        var authStatus = AuthStatus.Unauthorized;

        while (!_cancellationTokenSource.IsCancellationRequested &&
               Interlocked.Increment(ref _reconnectionAttempts) <=
               _reconnectionParameters.MaxReconnectionAttempts)
        {
#pragma warning disable CA5394 // Do not use insecure randomness
            await Task.Delay(_random.Next(
#pragma warning restore CA5394 // Do not use insecure randomness
                        (Int32)_reconnectionParameters.MinReconnectionDelay.TotalMilliseconds,
                        (Int32)_reconnectionParameters.MaxReconnectionDelay.TotalMilliseconds),
                    _cancellationTokenSource.Token)
                .ConfigureAwait(false);

            authStatus = await Client.ConnectAndAuthenticateAsync(_cancellationTokenSource.Token)
                .ConfigureAwait(false);

            if (authStatus == AuthStatus.Authorized)
            {
                break;
            }

            await Client.DisconnectAsync(_cancellationTokenSource.Token)
                .ConfigureAwait(false);
        }

        if (authStatus == AuthStatus.Authorized &&
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

        return authStatus;
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

    private Task handleErrorImpl(
        Exception exception)
    {
        switch (exception)
        {
            case SocketException socketException:
                return handleSocketException(
                    _retrySocketErrorCodes, socketException.SocketErrorCode, socketException);

            case WebSocketException webSocketException:
                return handleSocketException(_reconnectionParameters.RetryWebSocketErrorCodes,
                    webSocketException.WebSocketErrorCode, webSocketException);

            case RestClientErrorException:
                OnError?.Invoke(exception);
                break;

            case TaskCanceledException: // Expected one - don't report
                break;

            default:
                OnError?.Invoke(exception);
                return Client.DisconnectAsync(_cancellationTokenSource.Token);
        }

        return Task.CompletedTask;
    }

    private async Task<AuthStatus> runWithReconnection(
        Func<Task<AuthStatus>> action)
    {
        var errorOccurred = false;

        Client.OnError += HandleOnError;
        var authStatus = await action().ConfigureAwait(false);
        Client.OnError -= HandleOnError;

        if (errorOccurred && //-V3022
            authStatus == AuthStatus.Unauthorized)
        {
            return await handleSocketClosedAsync().ConfigureAwait(false);
        }

        return authStatus;

        void HandleOnError(Exception _) => errorOccurred = true;
    }

    private Task handleSocketException<TErrorCode>(
        ICollection<TErrorCode> retryErrorCodes,
        TErrorCode errorCode,
        Exception exception)
    {
        if (!retryErrorCodes.Contains(errorCode))
        {
            OnError?.Invoke(exception);
        }
        return Client.DisconnectAsync(_cancellationTokenSource.Token);
    }
}
