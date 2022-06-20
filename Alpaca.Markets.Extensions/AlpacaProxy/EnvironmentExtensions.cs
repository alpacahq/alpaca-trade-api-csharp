namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extensions methods for replacing target URL for Alpaca data streaming client (
/// <see cref="IAlpacaDataStreamingClient"/>) with custom values or with local proxy
/// WebSocket URL obtained from environment variables.
/// </summary>
public static class EnvironmentExtensions
{
    private const String EnvironmentVariableName = "DATA_PROXY_WS";

    private const String DefaultAlpacaProxyAgentUri = "ws://127.0.0.1:8765";

    private sealed class ProxyEnvironment : IEnvironment
    {
        private readonly IEnvironment _environment;

        public ProxyEnvironment(
            IEnvironment environment)
        {
            _environment = environment;
            AlpacaDataStreamingApi = _environment.AlpacaDataStreamingApi;
        }

        public Uri AlpacaDataStreamingApi { get; init; }

        public Uri AlpacaCryptoStreamingApi => _environment.AlpacaCryptoStreamingApi;

        public Uri AlpacaNewsStreamingApi => _environment.AlpacaNewsStreamingApi;

        public Uri AlpacaTradingApi => _environment.AlpacaTradingApi;

        public Uri AlpacaDataApi => _environment.AlpacaDataApi;

        public Uri AlpacaStreamingApi => _environment.AlpacaStreamingApi;
    }

    /// <summary>
    /// Replaces <see cref="IEnvironment.AlpacaDataStreamingApi"/> from environment
    /// variable named <c>DATA_PROXY_WS</c> with default fallback value equal to default
    /// Alpaca proxy agent local URL (<c>ws://127.0.0.1:8765</c>).
    /// </summary>
    /// <param name="environment">Original environment URLs for modification.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="environment"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>New environment URLs object.</returns>
    [UsedImplicitly]
    public static IEnvironment WithProxyForAlpacaDataStreamingClient(
        this IEnvironment environment) =>
        WithProxyForAlpacaDataStreamingClient(
            environment.EnsureNotNull(), getFromEnvironmentOrDefault());

    /// <summary>
    /// Replaces <see cref="IEnvironment.AlpacaDataStreamingApi"/> value with the
    /// <paramref name="alpacaProxyAgentUrl"/> value or from environment variable
    /// named <c>DATA_PROXY_WS</c> with default fallback value equal to default
    /// Alpaca proxy agent local URL (<c>ws://127.0.0.1:8765</c>).
    /// </summary>
    /// <param name="environment">Original environment URLs for modification.</param>
    /// <param name="alpacaProxyAgentUrl">
    /// New value for the <see cref="IEnvironment.AlpacaDataStreamingApi"/> property
    /// in the modified <paramref name="environment"/> object.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="environment"/> or <paramref name="alpacaProxyAgentUrl"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>New environment URLs object.</returns>
    [UsedImplicitly]
    public static IEnvironment WithProxyForAlpacaDataStreamingClient(
        this IEnvironment environment,
        Uri alpacaProxyAgentUrl) =>
        new ProxyEnvironment(environment.EnsureNotNull())
        {
            AlpacaDataStreamingApi = alpacaProxyAgentUrl.EnsureNotNull()
        };

    private static Uri getFromEnvironmentOrDefault() =>
        new(Environment.GetEnvironmentVariable(EnvironmentVariableName) //-V3022
            ?? DefaultAlpacaProxyAgentUri);
}
