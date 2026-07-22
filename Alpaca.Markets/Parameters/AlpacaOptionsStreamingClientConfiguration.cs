namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaOptionsStreamingClient"/> instance.
/// </summary>
public sealed class AlpacaOptionsStreamingClientConfiguration : StreamingClientConfiguration
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaOptionsStreamingClientConfiguration"/> class.
    /// </summary>
    public AlpacaOptionsStreamingClientConfiguration()
        : base(Environments.Live.AlpacaOptionsStreamingApi)
    {
        Feed = OptionsFeed.Indicative; // Default to free feed
        UseMessagePack = true;
    }

    /// <summary>
    /// Creates new instance of <see cref="AlpacaOptionsStreamingClientConfiguration"/> class.
    /// </summary>
    /// <param name="feed">Options data feed selection (Indicative or OPRA).</param>
    public AlpacaOptionsStreamingClientConfiguration(
        OptionsFeed feed)
        : base(Environments.Live.AlpacaOptionsStreamingApi)
    {
        Feed = feed;
        UseMessagePack = true;
    }

    private AlpacaOptionsStreamingClientConfiguration(
        AlpacaOptionsStreamingClientConfiguration configuration,
        OptionsFeed feed)
        : base(configuration.ApiEndpoint)
    {
        SecurityId = configuration.SecurityId;
        Feed = feed;
        UseMessagePack = true;
    }

    /// <summary>
    /// Gets options' data feed selection (Indicative or OPRA).
    /// </summary>
    [UsedImplicitly]
    public OptionsFeed Feed { get; private set; }

    /// <summary>
    /// Creates new instance of <see cref="AlpacaOptionsStreamingClientConfiguration"/> object
    /// with the updated <see cref="Feed"/> value.
    /// </summary>
    /// <param name="feed">Options data feed selection.</param>
    /// <returns>The new instance of the <see cref="AlpacaOptionsStreamingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public AlpacaOptionsStreamingClientConfiguration WithFeed(
        OptionsFeed feed) =>
        new(this, feed);

    internal override Uri GetApiEndpoint()
    {
        var baseUrl = base.GetApiEndpoint();
        // Options streaming API uses format: wss://stream.data.alpaca.markets/v1beta1/{feed}
        // where {feed} is either "indicative" or "opra"
        var feedValue = Feed == OptionsFeed.Opra ? "opra" : "indicative";
        return new Uri(baseUrl, feedValue);
    }
}