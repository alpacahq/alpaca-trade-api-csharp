namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="AlpacaOptionsStreamingClient"/> instance.
/// </summary>
public sealed class AlpacaOptionsStreamingClientConfiguration : StreamingClientConfiguration
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaOptionsStreamingClientConfiguration"/> class.
    /// </summary>
    public AlpacaOptionsStreamingClientConfiguration()
        : base(Environments.Live.AlpacaOptionsStreamingApi)
    {
    }
}
