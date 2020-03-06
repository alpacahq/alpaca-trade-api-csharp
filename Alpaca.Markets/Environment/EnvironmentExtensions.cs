using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Collection of helper extension methods for <see cref="IEnvironment"/> interface.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class EnvironmentExtensions
    {
        /// <summary>
        /// Creates new instance of <see cref="AlpacaTradingClient"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <param name="securityKey">Alpaca API security key.</param>
        /// <returns>New instance of <see cref="AlpacaTradingClient"/> object.</returns>
        public static AlpacaTradingClient GetAlpacaTradingClient(
            this IEnvironment environment,
            String keyId,
            SecurityKey securityKey) =>
            new AlpacaTradingClient(environment.GetAlpacaTradingClientConfiguration(keyId, securityKey));

        /// <summary>
        /// Creates new instance of <see cref="AlpacaTradingClientConfiguration"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <param name="securityKey">Alpaca API security key.</param>
        /// <returns>New instance of <see cref="AlpacaTradingClientConfiguration"/> object.</returns>
        public static AlpacaTradingClientConfiguration GetAlpacaTradingClientConfiguration(
            this IEnvironment environment,
            String keyId,
            SecurityKey securityKey) =>
            new AlpacaTradingClientConfiguration
            {
                ApiEndpoint = environment?.AlpacaTradingApi ?? throw new ArgumentNullException(nameof(environment)),
                SecurityId = securityKey ?? throw new ArgumentNullException(nameof(securityKey)),
                KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId))
            };

        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataClient"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <param name="securityKey">Alpaca API security key.</param>
        /// <returns>New instance of <see cref="AlpacaDataClient"/> object.</returns>
        public static AlpacaDataClient GetAlpacaDataClient(
            this IEnvironment environment,
            String keyId,
            SecurityKey securityKey) =>
            new AlpacaDataClient(environment.GetAlpacaDataClientConfiguration(keyId, securityKey));

        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataClientConfiguration"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <param name="securityKey">Alpaca API security key.</param>
        /// <returns>New instance of <see cref="AlpacaDataClientConfiguration"/> object.</returns>
        public static AlpacaDataClientConfiguration GetAlpacaDataClientConfiguration(
            this IEnvironment environment,
            String keyId,
            SecurityKey securityKey) =>
            new AlpacaDataClientConfiguration
            {
                ApiEndpoint = environment?.AlpacaDataApi ?? throw new ArgumentNullException(nameof(environment)),
                SecurityId = securityKey ?? throw new ArgumentNullException(nameof(securityKey)),
                KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId))
            };

        /// <summary>
        /// Creates new instance of <see cref="PolygonDataClient"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <returns>New instance of <see cref="PolygonDataClient"/> object.</returns>
        public static PolygonDataClient GetPolygonDataClient(
            this IEnvironment environment,
            String keyId) =>
            new PolygonDataClient(environment.GetPolygonDataClientConfiguration(keyId));

        /// <summary>
        /// Creates new instance of <see cref="PolygonDataClientConfiguration"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <returns>New instance of <see cref="PolygonDataClientConfiguration"/> object.</returns>
        public static PolygonDataClientConfiguration GetPolygonDataClientConfiguration(
            this IEnvironment environment,
            String keyId) =>
            new PolygonDataClientConfiguration
            {
                ApiEndpoint = environment?.PolygonDataApi ?? throw new ArgumentNullException(nameof(environment)),
                KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId))
            };

        /// <summary>
        /// Creates new instance of <see cref="AlpacaStreamingClient"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <param name="secretKey">Alpaca API secret key.</param>
        /// <param name="oauthToken">Alpaca API oauth token. If provided, key and secret key will be ignored.</param>
        /// <returns>New instance of <see cref="AlpacaStreamingClient"/> object.</returns>
        public static AlpacaStreamingClient GetAlpacaStreamingClient(
            this IEnvironment environment,
            String? keyId = null,
            String? secretKey = null,
            String? oauthToken = null) =>
            new AlpacaStreamingClient(environment.GetAlpacaStreamingClientConfiguration(keyId, secretKey, oauthToken));

        /// <summary>
        /// Creates new instance of <see cref="AlpacaStreamingClientConfiguration"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <param name="secretKey">Alpaca API secret key.</param>
        /// <param name="oauthToken">Alpaca API oauth token. If provided, key and secret key will be ignored.</param>
        /// <returns>New instance of <see cref="AlpacaStreamingClientConfiguration"/> object.</returns>
        public static AlpacaStreamingClientConfiguration GetAlpacaStreamingClientConfiguration(
            this IEnvironment environment,
            String? keyId = null,
            String? secretKey = null,
            String? oauthToken = null) =>
            new AlpacaStreamingClientConfiguration()
            {
                ApiEndpoint = environment?.AlpacaStreamingApi ?? throw new ArgumentNullException(nameof(environment)),
                SecretKey = secretKey ?? oauthToken ?? throw new ArgumentNullException(nameof(secretKey)),
                KeyId = keyId ?? oauthToken ?? throw new ArgumentNullException(nameof(keyId)),
                OAuthToken = oauthToken ?? String.Empty
            };

        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClient"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <returns>New instance of <see cref="PolygonStreamingClient"/> object.</returns>
        public static PolygonStreamingClient GetPolygonStreamingClient(
            this IEnvironment environment,
            String keyId) =>
            new PolygonStreamingClient(environment.GetPolygonStreamingClientConfiguration(keyId));

        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClientConfiguration"/> for specific
        /// environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <returns>New instance of <see cref="PolygonStreamingClientConfiguration"/> object.</returns>
        public static PolygonStreamingClientConfiguration GetPolygonStreamingClientConfiguration(
            this IEnvironment environment,
            String keyId) =>
            new PolygonStreamingClientConfiguration()
            {
                ApiEndpoint = environment?.PolygonStreamingApi ?? throw new ArgumentNullException(nameof(environment)),
                KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId))
            };
    }
}
