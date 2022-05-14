namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaStreamingClient"/> instance.
/// </summary>
public sealed class AlpacaStreamingClientConfiguration : StreamingClientConfiguration
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaStreamingClientConfiguration"/> class.
    /// </summary>
    public AlpacaStreamingClientConfiguration()
        : base(Environments.Live.AlpacaStreamingApi)
    {
    }
}
