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
        /// Creates the new instance of <see cref="IAlpacaTradingClient"/> interface
        /// implementation for specific environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="securityKey">Alpaca API security key.</param>
        /// <returns>The new instance of <see cref="IAlpacaTradingClient"/> interface implementation.</returns>
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
        public static AlpacaTradingClientConfiguration GetAlpacaTradingClientConfiguration(
            this IEnvironment environment,
            SecurityKey securityKey) =>
            new AlpacaTradingClientConfiguration
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
        public static AlpacaDataClientConfiguration GetAlpacaDataClientConfiguration(
            this IEnvironment environment,
            SecurityKey securityKey) =>
            new AlpacaDataClientConfiguration
            {
                ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                    .AlpacaDataApi.EnsureNotNull(nameof(IEnvironment.AlpacaDataApi)),
                SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
            };

        /// <summary>
        /// Creates the new instance of <see cref="IPolygonDataClient"/> interface
        /// implementation for specific environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <returns>The new instance of <see cref="IPolygonDataClient"/> interface implementation.</returns>
        public static IPolygonDataClient GetPolygonDataClient(
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
                ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                    .PolygonDataApi.EnsureNotNull(nameof(IEnvironment.PolygonDataApi)),
                KeyId = keyId ?? throw new ArgumentNullException(nameof(keyId))
            };

        /// <summary>
        /// Creates the new instance of <see cref="IAlpacaStreamingClient"/> interface
        /// implementation for specific environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="securityKey">Alpaca API security key.</param>
        /// <returns>The new instance of <see cref="IAlpacaStreamingClient"/> interface implementation.</returns>
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
        public static AlpacaStreamingClientConfiguration GetAlpacaStreamingClientConfiguration(
            this IEnvironment environment,
            SecurityKey securityKey) =>
            new AlpacaStreamingClientConfiguration
            {
                ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                    .AlpacaStreamingApi.EnsureNotNull(nameof(IEnvironment.AlpacaStreamingApi)),
                SecurityId = securityKey
            };

        /// <summary>
        /// Creates the new instance of <see cref="IPolygonStreamingClient"/> interface
        /// implementation for specific environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="keyId">Alpaca API key identifier.</param>
        /// <returns>The new instance of <see cref="IPolygonStreamingClient"/> interface implementation.</returns>
        public static IPolygonStreamingClient GetPolygonStreamingClient(
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
            new PolygonStreamingClientConfiguration
            {
                ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                    .PolygonStreamingApi.EnsureNotNull(nameof(IEnvironment.PolygonStreamingApi)),
                KeyId = keyId.EnsureNotNull(nameof(keyId))
            };

        /// <summary>
        /// Creates the new instance of <see cref="IAlpacaDataStreamingClient"/> interface
        /// implementation for specific environment provided as <paramref name="environment"/> argument.
        /// </summary>
        /// <param name="environment">Target environment for new object.</param>
        /// <param name="securityKey">Alpaca API security key.</param>
        /// <returns>The new instance of <see cref="IAlpacaDataStreamingClient"/> interface implementation.</returns>
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
        public static AlpacaDataStreamingClientConfiguration GetAlpacaDataStreamingClientConfiguration(
            this IEnvironment environment,
            SecurityKey securityKey) =>
            new AlpacaDataStreamingClientConfiguration
            {
                ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                    .AlpacaDataStreamingApi.EnsureNotNull(nameof(IEnvironment.AlpacaDataStreamingApi)),
                SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
            };
    }
}
