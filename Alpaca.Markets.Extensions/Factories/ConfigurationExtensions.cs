using System;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extensions methods for creating the strongly-typed Alpaca REST API clients.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Creates the new instance of <see cref="IAlpacaCryptoDataClient"/> interface
        /// implementation using the <paramref name="configuration"/> argument.
        /// </summary>
        /// <param name="configuration">Client configuration parameters.</param>
        /// <returns>The new instance of <see cref="IAlpacaCryptoDataClient"/> interface implementation.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaCryptoDataClient GetClient(
            this AlpacaCryptoDataClientConfiguration configuration) =>
            new AlpacaCryptoDataClient(configuration);

        /// <summary>
        /// Creates the new instance of <see cref="IAlpacaDataClient"/> interface
        /// implementation using the <paramref name="configuration"/> argument.
        /// </summary>
        /// <param name="configuration">Client configuration parameters.</param>
        /// <returns>The new instance of <see cref="IAlpacaDataClient"/> interface implementation.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataClient GetClient(
            this AlpacaDataClientConfiguration configuration) =>
            new AlpacaDataClient(configuration);

        /// <summary>
        /// Creates the new instance of <see cref="IAlpacaDataStreamingClient"/> interface
        /// implementation using the <paramref name="configuration"/> argument.
        /// </summary>
        /// <param name="configuration">Client configuration parameters.</param>
        /// <returns>The new instance of <see cref="IAlpacaDataStreamingClient"/> interface implementation.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaDataStreamingClient GetClient(
            this AlpacaDataStreamingClientConfiguration configuration) =>
            new AlpacaDataStreamingClient(configuration);

        /// <summary>
        /// Creates the new instance of <see cref="IAlpacaStreamingClient"/> interface
        /// implementation using the <paramref name="configuration"/> argument.
        /// </summary>
        /// <param name="configuration">Client configuration parameters.</param>
        /// <returns>The new instance of <see cref="IAlpacaStreamingClient"/> interface implementation.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaStreamingClient GetClient(
            this AlpacaStreamingClientConfiguration configuration) =>
            new AlpacaStreamingClient(configuration);

        /// <summary>
        /// Creates the new instance of <see cref="IAlpacaTradingClient"/> interface
        /// implementation using the <paramref name="configuration"/> argument.
        /// </summary>
        /// <param name="configuration">Client configuration parameters.</param>
        /// <returns>The new instance of <see cref="IAlpacaTradingClient"/> interface implementation.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAlpacaTradingClient GetClient(
            this AlpacaTradingClientConfiguration configuration) =>
            new AlpacaTradingClient(configuration);
    }
}
