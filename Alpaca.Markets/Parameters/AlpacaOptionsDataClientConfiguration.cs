namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaOptionsDataClient"/> instance.
/// </summary>
public sealed class AlpacaOptionsDataClientConfiguration : AlpacaClientConfigurationBase
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaDataClientConfiguration"/> class.
    /// </summary>
    public AlpacaOptionsDataClientConfiguration()
        : base(Environments.Live.AlpacaDataApi)
    {
    }

    internal override Uri GetApiEndpoint() =>
        new UriBuilder(ApiEndpoint) { Path = "v1beta1/options/" }.Uri;
}
