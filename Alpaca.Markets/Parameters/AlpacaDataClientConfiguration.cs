namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaDataClient"/> instance.
/// </summary>
public sealed class AlpacaDataClientConfiguration : AlpacaClientConfigurationBase
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaDataClientConfiguration"/> class.
    /// </summary>
    public AlpacaDataClientConfiguration()
        : base(Environments.Live.AlpacaDataApi)
    {
    }

    internal override Uri GetApiEndpoint() =>
        new UriBuilder(ApiEndpoint) { Path = "v2/stocks/" }.Uri;
}
