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
        var feedValue = Feed == OptionsFeed.Opra ? "opra" : "indicative";
        // Construct the endpoint by treating the feed name as a relative URI.
        // When baseUrl ends in /v1beta1/opra or /v1beta1/indicative, the relative
        // path "opra" or "indicative" replaces the last segment, which correctly
        // switches between feeds without duplicating the /v1beta1 prefix.
        // This approach works for the designed environments (Live and Paper).
        return new Uri(base.GetApiEndpoint(), feedValue);
    }
}