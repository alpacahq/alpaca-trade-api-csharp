using System.Net.Http;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extensions methods for registering the strongly-typed Alpaca REST API clients
    /// in the default Microsoft dependency injection container used by the most .NET hosts.
    /// </summary>
    public static class AlpacaServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the concrete implementation of the <see cref="IAlpacaCryptoDataClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="environment">Alpaca environment data.</param>
        /// <param name="securityKey">Alpaca security key.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        [UsedImplicitly]
        public static IServiceCollection AddAlpacaCryptoDataClient(
            this IServiceCollection services,
            IEnvironment environment,
            SecurityKey securityKey) =>
            services.AddAlpacaCryptoDataClient(environment
                .GetAlpacaCryptoDataClientConfiguration(securityKey));

        /// <summary>
        /// Registers the concrete implementation of the <see cref="IAlpacaCryptoDataClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="configuration">Alpaca data client configuration.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        [UsedImplicitly]
        public static IServiceCollection AddAlpacaCryptoDataClient(
            this IServiceCollection services,
            AlpacaCryptoDataClientConfiguration configuration) =>
            services
                .AddHttpClient<IAlpacaCryptoDataClient>()
                .AddTypedClient<IAlpacaCryptoDataClient>(
                    httpClient => new AlpacaCryptoDataClient(
                        configuration.withFactoryCreatedHttpClient(httpClient)))
                .ConfigurePrimaryHttpMessageHandler(() => configuration
                    .EnsureNotNull(nameof(configuration))
                    .ThrottleParameters.GetMessageHandler())
                .Services;

        /// <summary>
        /// Registers the concrete implementation of the <see cref="IAlpacaDataClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="environment">Alpaca environment data.</param>
        /// <param name="securityKey">Alpaca security key.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        [UsedImplicitly]
        public static IServiceCollection AddAlpacaDataClient(
            this IServiceCollection services,
            IEnvironment environment,
            SecurityKey securityKey) =>
            services.AddAlpacaDataClient(environment
                .GetAlpacaDataClientConfiguration(securityKey));

        /// <summary>
        /// Registers the concrete implementation of the <see cref="IAlpacaDataClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="configuration">Alpaca data client configuration.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        [UsedImplicitly]
        public static IServiceCollection AddAlpacaDataClient(
            this IServiceCollection services,
            AlpacaDataClientConfiguration configuration) =>
            services
                .AddHttpClient<IAlpacaDataClient>()
                .AddTypedClient<IAlpacaDataClient>(
                    httpClient => new AlpacaDataClient(
                        configuration.withFactoryCreatedHttpClient(httpClient)))
                .ConfigurePrimaryHttpMessageHandler(() => configuration
                    .EnsureNotNull(nameof(configuration))
                    .ThrottleParameters.GetMessageHandler())
                .Services;

        /// <summary>
        /// Registers the concrete implementation of the <see cref="IAlpacaTradingClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="environment">Alpaca environment data.</param>
        /// <param name="securityKey">Alpaca security key.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        [UsedImplicitly]
        public static IServiceCollection AddAlpacaTradingClient(
            this IServiceCollection services,
            IEnvironment environment,
            SecurityKey securityKey) =>
            services.AddAlpacaTradingClient(environment
                .GetAlpacaTradingClientConfiguration(securityKey));

        /// <summary>
        /// Registers the concrete implementation of the <see cref="IAlpacaTradingClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="configuration">Alpaca trading client configuration.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        [UsedImplicitly]
        public static IServiceCollection AddAlpacaTradingClient(
            this IServiceCollection services,
            AlpacaTradingClientConfiguration configuration) =>
            services
                .AddHttpClient<IAlpacaTradingClient>()
                .AddTypedClient<IAlpacaTradingClient>(
                    httpClient => new AlpacaTradingClient(
                        configuration.withFactoryCreatedHttpClient(httpClient)))
                .ConfigurePrimaryHttpMessageHandler(() => configuration
                    .EnsureNotNull(nameof(configuration))
                    .ThrottleParameters.GetMessageHandler())
                .Services;

        private static AlpacaCryptoDataClientConfiguration withFactoryCreatedHttpClient(
            this AlpacaCryptoDataClientConfiguration configuration,
            HttpClient httpClient)
        {
            configuration.HttpClient = httpClient;
            return configuration;
        }

        private static AlpacaDataClientConfiguration withFactoryCreatedHttpClient(
            this AlpacaDataClientConfiguration configuration,
            HttpClient httpClient)
        {
            configuration.HttpClient = httpClient;
            return configuration;
        }

        private static AlpacaTradingClientConfiguration withFactoryCreatedHttpClient(
            this AlpacaTradingClientConfiguration configuration,
            HttpClient httpClient)
        {
            configuration.HttpClient = httpClient;
            return configuration;
        }
    }
}
