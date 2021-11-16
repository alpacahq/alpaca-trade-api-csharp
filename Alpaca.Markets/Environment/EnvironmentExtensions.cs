namespace Alpaca.Markets;

/// <summary>
/// Collection of helper extension methods for <see cref="IEnvironment"/> interface.
/// </summary>
public static class EnvironmentExtensions
{
    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaTradingClient"/> interface
    /// implementation for specific environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>The new instance of <see cref="IAlpacaTradingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaTradingClient GetAlpacaTradingClient(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new AlpacaTradingClient(environment.GetAlpacaTradingClientConfiguration(securityKey));

    /// <summary>
    /// Creates new instance of <see cref="AlpacaTradingClientConfiguration"/> for specific
    /// environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>New instance of <see cref="AlpacaTradingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public static AlpacaTradingClientConfiguration GetAlpacaTradingClientConfiguration(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new()
        {
            ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                .AlpacaTradingApi.EnsureNotNull(nameof(IEnvironment.AlpacaTradingApi)),
            SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
        };

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaDataClient"/> interface
    /// implementation for specific environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>The new instance of <see cref="IAlpacaDataClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataClient GetAlpacaDataClient(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new AlpacaDataClient(environment.GetAlpacaDataClientConfiguration(securityKey));

    /// <summary>
    /// Creates new instance of <see cref="AlpacaDataClientConfiguration"/> for specific
    /// environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>New instance of <see cref="AlpacaDataClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public static AlpacaDataClientConfiguration GetAlpacaDataClientConfiguration(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new()
        {
            ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                .AlpacaDataApi.EnsureNotNull(nameof(IEnvironment.AlpacaDataApi)),
            SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
        };

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaCryptoDataClient"/> interface
    /// implementation for specific environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>The new instance of <see cref="IAlpacaCryptoDataClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaCryptoDataClient GetAlpacaCryptoDataClient(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new AlpacaCryptoDataClient(environment.GetAlpacaCryptoDataClientConfiguration(securityKey));

    /// <summary>
    /// Creates new instance of <see cref="AlpacaCryptoDataClientConfiguration"/> for specific
    /// environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>New instance of <see cref="AlpacaCryptoDataClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public static AlpacaCryptoDataClientConfiguration GetAlpacaCryptoDataClientConfiguration(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new()
        {
            ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                .AlpacaDataApi.EnsureNotNull(nameof(IEnvironment.AlpacaDataApi)),
            SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
        };

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaStreamingClient"/> interface
    /// implementation for specific environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>The new instance of <see cref="IAlpacaStreamingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaStreamingClient GetAlpacaStreamingClient(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new AlpacaStreamingClient(environment.GetAlpacaStreamingClientConfiguration(securityKey));

    /// <summary>
    /// Creates new instance of <see cref="AlpacaStreamingClientConfiguration"/> for specific
    /// environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>New instance of <see cref="AlpacaStreamingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public static AlpacaStreamingClientConfiguration GetAlpacaStreamingClientConfiguration(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new()
        {
            ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                .AlpacaStreamingApi.EnsureNotNull(nameof(IEnvironment.AlpacaStreamingApi)),
            SecurityId = securityKey
        };

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaDataStreamingClient"/> interface
    /// implementation for specific environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>The new instance of <see cref="IAlpacaDataStreamingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataStreamingClient GetAlpacaDataStreamingClient(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new AlpacaDataStreamingClient(environment.GetAlpacaDataStreamingClientConfiguration(securityKey));

    /// <summary>
    /// Creates new instance of <see cref="AlpacaDataStreamingClientConfiguration"/> for specific
    /// environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>New instance of <see cref="AlpacaDataStreamingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public static AlpacaDataStreamingClientConfiguration GetAlpacaDataStreamingClientConfiguration(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new()
        {
            ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                .AlpacaDataStreamingApi.EnsureNotNull(nameof(IEnvironment.AlpacaDataStreamingApi)),
            SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
        };

    /// <summary>
    /// Creates the new instance of <see cref="IAlpacaCryptoStreamingClient"/> interface
    /// implementation for specific environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>The new instance of <see cref="IAlpacaCryptoStreamingClient"/> interface implementation.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaCryptoStreamingClient GetAlpacaCryptoStreamingClient(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new AlpacaCryptoStreamingClient(environment.GetAlpacaCryptoStreamingClientConfiguration(securityKey));

    /// <summary>
    /// Creates new instance of <see cref="AlpacaCryptoStreamingClientConfiguration"/> for specific
    /// environment provided as <paramref name="environment"/> argument.
    /// </summary>
    /// <param name="environment">Target environment for new object.</param>
    /// <param name="securityKey">Alpaca API security key.</param>
    /// <returns>New instance of <see cref="AlpacaCryptoStreamingClientConfiguration"/> object.</returns>
    [UsedImplicitly]
    public static AlpacaCryptoStreamingClientConfiguration GetAlpacaCryptoStreamingClientConfiguration(
        this IEnvironment environment,
        SecurityKey securityKey) =>
        new()
        {
            ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                .AlpacaCryptoStreamingApi.EnsureNotNull(nameof(IEnvironment.AlpacaCryptoStreamingApi)),
            SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
        };
}
