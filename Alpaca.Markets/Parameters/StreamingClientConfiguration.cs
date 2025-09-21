namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="StreamingClientBase{TConfiguration}"/> class.
/// </summary>
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public abstract class StreamingClientConfiguration
{
    /// <summary>
    /// Creates new instance of <see cref="StreamingClientConfiguration"/> class.
    /// </summary>
    protected internal StreamingClientConfiguration(Uri apiEndpoint)
    {
        SecurityId = new SecretKey(String.Empty, String.Empty);
        ApiEndpoint = apiEndpoint;
    }

    /// <summary>
    /// Gets or sets Alpaca streaming API base URL.
    /// </summary>
    public Uri ApiEndpoint { get; set; }

    /// <summary>
    /// Gets or sets Alpaca secret key identifier.
    /// </summary>
    public SecurityKey SecurityId { get; set; }

    /// <summary>
    /// Gets or sets factory for obtaining web socket client.
    /// </summary>
    public Func<IWebSocket>? WebSocketFactory { get; [UsedImplicitly] set; }

    /// <summary>
    /// Gets or sets the flag indicating that MessagePack format should be used.
    /// Limited the set accessor accessibility level to internal because specifying the message format using
    /// Content-Type header is not supported by the existing WebSocket implementation used.
    /// </summary>
    internal bool UseMessagePack { get; set; }

    internal virtual Uri GetApiEndpoint() => ApiEndpoint;

    internal void EnsureIsValid()
    {
        ApiEndpoint.EnsurePropertyNotNull();
        SecurityId.EnsurePropertyNotNull();
    }
}
