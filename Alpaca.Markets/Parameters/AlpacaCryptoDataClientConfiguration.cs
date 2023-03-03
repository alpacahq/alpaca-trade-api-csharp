namespace Alpaca.Markets;

/// <summary>
/// Configuration parameters object for <see cref="IAlpacaCryptoDataClient"/> instance.
/// </summary>
public sealed class AlpacaCryptoDataClientConfiguration : AlpacaClientConfigurationBase
{
    /// <summary>
    /// Creates new instance of <see cref="AlpacaCryptoDataClientConfiguration"/> class.
    /// </summary>
    public AlpacaCryptoDataClientConfiguration()
        : base(Environments.Live.AlpacaDataApi)
    {
    }

    internal override Uri GetApiEndpoint() =>
        
        new UriBuilder(ApiEndpoint) { Path = "v1beta3/crypto/us/" }.Uri;
}
