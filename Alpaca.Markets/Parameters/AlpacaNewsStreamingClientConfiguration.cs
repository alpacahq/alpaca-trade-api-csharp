namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaNewsStreamingClient"/> implementation.
/// </summary>
public sealed class AlpacaNewsStreamingClientConfiguration : StreamingClientConfiguration
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaNewsStreamingClientConfiguration"/> class.
    /// </summary>
    public AlpacaNewsStreamingClientConfiguration()
        : base(Environments.Live.AlpacaNewsStreamingApi)
    {
    }
}
