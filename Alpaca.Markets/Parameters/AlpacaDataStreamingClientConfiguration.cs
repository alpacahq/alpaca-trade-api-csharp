namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaDataStreamingClient"/> instance.
/// </summary>
public sealed class AlpacaDataStreamingClientConfiguration : StreamingClientConfiguration
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaDataStreamingClientConfiguration"/> class.
    /// </summary>
    public AlpacaDataStreamingClientConfiguration()
        : base(Environments.Live.AlpacaDataStreamingApi)
    {
    }

    private AlpacaDataStreamingClientConfiguration(
        AlpacaDataStreamingClientConfiguration configuration,
        MarketDataFeed feed)
        : base(configuration.ApiEndpoint)
    {
        SecurityId = configuration.SecurityId;
        WebSocketFactory = configuration.WebSocketFactory;
        Feed = feed;
    }

    /// <summary>
    /// Gets the real-time market data feed (for example, <see cref="MarketDataFeed.Iex"/> or
    /// <see cref="MarketDataFeed.Sip"/>) used for the data streaming connection. When this property
    /// is <c>null</c> the feed encoded in the <see cref="StreamingClientConfiguration.ApiEndpoint"/>
    /// (provided by the selected <see cref="IEnvironment"/>) is used as-is.
    /// </summary>
    [UsedImplicitly]
    public MarketDataFeed? Feed { get; private set; }

    /// <summary>
    /// Creates new instance of <see cref="AlpacaDataStreamingClientConfiguration"/> object
    /// with the updated <see cref="Feed"/> value.
    /// </summary>
    /// <param name="feed">Real-time market data feed selection (IEX, SIP, etc.).</param>
    /// <returns>The new instance of the <see cref="AlpacaDataStreamingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public AlpacaDataStreamingClientConfiguration WithFeed(
        MarketDataFeed feed) =>
        new(this, feed);

    internal override Uri GetApiEndpoint() =>
        Feed.HasValue
            // Stock data streaming API uses the format wss://stream.data.alpaca.markets/v2/{feed}
            // where {feed} is one of "iex", "sip", "delayed_sip", "boats" or "overnight".
            ? new UriBuilder(ApiEndpoint) { Path = $"v2/{Feed.Value.ToEnumString()}" }.Uri
            : base.GetApiEndpoint();
}
