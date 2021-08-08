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
#pragma warning disable 618
            new AlpacaTradingClient(environment.GetAlpacaTradingClientConfiguration(securityKey));
#pragma warning restore 618

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
            new ()
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
#pragma warning disable CS0618 // Type or member is obsolete
            new AlpacaDataClient(environment.GetAlpacaDataClientConfiguration(securityKey));
#pragma warning restore CS0618 // Type or member is obsolete

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
            new ()
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
        public static IAlpacaStreamingClient GetAlpacaStreamingClient(
            this IEnvironment environment,
            SecurityKey securityKey) =>
#pragma warning disable 618
            new AlpacaStreamingClient(environment.GetAlpacaStreamingClientConfiguration(securityKey));
#pragma warning restore 618

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
            new ()
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
        public static IAlpacaDataStreamingClient GetAlpacaDataStreamingClient(
            this IEnvironment environment,
            SecurityKey securityKey) =>
#pragma warning disable 618
            new AlpacaDataStreamingClient(environment.GetAlpacaDataStreamingClientConfiguration(securityKey));
#pragma warning restore 618

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
            new ()
            {
                ApiEndpoint = environment.EnsureNotNull(nameof(environment))
                    .AlpacaDataStreamingApi.EnsureNotNull(nameof(IEnvironment.AlpacaDataStreamingApi)),
                SecurityId = securityKey.EnsureNotNull(nameof(securityKey))
            };
    }
}
