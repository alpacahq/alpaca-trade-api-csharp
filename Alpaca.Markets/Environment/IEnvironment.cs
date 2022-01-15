namespace Alpaca.Markets;

/// <summary>
/// Provides URLs for different APIs available for this SDK on specific environment.
/// </summary>
public interface IEnvironment
{
    /// <summary>
    /// Gets Alpaca trading REST API base URL for this environment.
    /// </summary>
    Uri AlpacaTradingApi { get; }

    /// <summary>
    /// Gets Alpaca data REST API base URL for this environment.
    /// </summary>
    Uri AlpacaDataApi { get; }

    /// <summary>
    /// Gets Alpaca streaming API base URL for this environment.
    /// </summary>
    Uri AlpacaStreamingApi { get; }

    /// <summary>
    /// Gets Alpaca data streaming API base URL for this environment.
    /// </summary>
    Uri AlpacaDataStreamingApi { get; }

    /// <summary>
    /// Gets Alpaca crypto streaming API base URL for this environment.
    /// </summary>
    Uri AlpacaCryptoStreamingApi { get; }

    /// <summary>
    /// Gets Alpaca news streaming API base URL for this environment.
    /// </summary>
    Uri AlpacaNewsStreamingApi { get; }
}
