using System.Text;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets;

internal abstract class StreamingClientBase<TConfiguration> : IStreamingClient
    where TConfiguration : StreamingClientConfiguration
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Func<IWebSocket> _webSocketFactory =
        () => new ClientWebSocketWrapper();

    private readonly SynchronizationQueue _queue = new();

    internal readonly TConfiguration Configuration;

    private readonly WebSocketsTransport _webSocket;

    private protected StreamingClientBase(
        TConfiguration configuration)
    {
        Configuration = configuration.EnsureNotNull();
        Configuration.EnsureIsValid();

        _webSocket = new WebSocketsTransport(
            Configuration.WebSocketFactory ?? _webSocketFactory,
            Configuration.GetApiEndpoint());

        _webSocket.Opened += OnOpened;
        _webSocket.Closed += OnClosed;

        _webSocket.MessageReceived += OnMessageReceived;
        _webSocket.DataReceived += onDataReceived;

        _webSocket.Error += HandleError;
        _queue.OnError += HandleError;
    }

    public event Action<AuthStatus>? Connected;

    public event Action? SocketOpened;

    public event Action? SocketClosed;

    public event Action<String>? OnWarning;

    public event Action<Exception>? OnError;

    public Task ConnectAsync(
        CancellationToken cancellationToken = default)
        => _webSocket.StartAsync(cancellationToken);

    public async Task<AuthStatus> ConnectAndAuthenticateAsync(
        CancellationToken cancellationToken = default)
    {
        var tcs = new TaskCompletionSource<AuthStatus>(TaskCreationOptions.RunContinuationsAsynchronously);
        Connected += HandleConnected;
        OnError += HandleOnError;

        await ConnectAsync(cancellationToken).ConfigureAwait(false);
        return await tcs.Task.ConfigureAwait(false);

        void HandleConnected(AuthStatus authStatus)
        {
            Connected -= HandleConnected;
            OnError -= HandleOnError;

            tcs.SetResult(authStatus);
        }

        void HandleOnError(Exception exception) =>
            HandleConnected(
                exception is SocketException { SocketErrorCode: SocketError.IsConnected }
                    ? AuthStatus.Authorized
                    : AuthStatus.Unauthorized);
    }

    /// <inheritdoc />
    public Task DisconnectAsync(
        CancellationToken cancellationToken = default)
        => _webSocket.StopAsync(cancellationToken);

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void OnOpened() => SocketOpened?.Invoke();

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void OnClosed() => SocketClosed?.Invoke();

    [ExcludeFromCodeCoverage]
    protected virtual void OnMessageReceived(
        String message)
    {
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(
        Boolean disposing)
    {
        if (!disposing)
        {
            return;
        }

        _webSocket.Opened -= OnOpened;
        _webSocket.Closed -= OnClosed;

        _webSocket.MessageReceived -= OnMessageReceived;
        _webSocket.DataReceived -= onDataReceived;

        _webSocket.Error -= HandleError;
        _queue.OnError -= OnError;

        _webSocket.Dispose();
        _queue.Dispose();
    }

    [SuppressMessage(
        "Design", "CA1031:Do not catch general exception types",
        Justification = "Expected behavior - we report exceptions via OnError event.")]
    protected void HandleMessage<TKey>(
        IDictionary<TKey, Action<JToken>> handlers,
        TKey messageType,
        JToken message)
        where TKey : class
    {
        try
        {
            if (handlers.EnsureNotNull().TryGetValue(messageType, out var handler))
            {
                _queue.Enqueue(() => handler(message));
            }
            else
            {
                HandleWarning($"Unexpected message type '{messageType}' received.");
            }
        }
        catch (Exception exception)
        {
            HandleError(exception);
        }
    }

    protected void OnConnected(
        AuthStatus authStatus) =>
        Connected?.Invoke(authStatus);

    protected void HandleError(
        Exception exception)
    {
        if (exception is SocketException { SocketErrorCode: SocketError.IsConnected })
        {
            return; // We skip that error because it doesn't matter for us
        }
        OnError?.Invoke(exception);
    }

    protected void HandleWarning(
        String message) => 
        OnWarning?.Invoke(message);

    protected ValueTask SendAsJsonStringAsync(
        Object value,
        CancellationToken cancellationToken = default)
    {
        using var textWriter = new StringWriter();

        var serializer = new JsonSerializer();
        serializer.Serialize(textWriter, value);
        return _webSocket.SendAsync(textWriter.ToString(), cancellationToken);
    }

#pragma warning disable IDE1006 // Naming Styles
    private void onDataReceived(
#pragma warning restore IDE1006 // Naming Styles
        Byte[] binaryData) =>
        OnMessageReceived(Encoding.UTF8.GetString(binaryData));
}
