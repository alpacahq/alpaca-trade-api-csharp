namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaTradingClient"/> instance.
/// </summary>
public sealed class AlpacaTradingClientConfiguration : AlpacaClientConfigurationBase
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaTradingClientConfiguration"/> class.
    /// </summary>
    public AlpacaTradingClientConfiguration()
        : base(Environments.Live.AlpacaTradingApi)
    {
    }

    internal override Uri GetApiEndpoint() => ApiEndpoint;
}
