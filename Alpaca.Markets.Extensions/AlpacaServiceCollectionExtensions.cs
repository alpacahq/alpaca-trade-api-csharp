using System.Net.Http;
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
        /// Registers the concrete implementation of the <see cref="IAlpacaDataClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="environment">Alpaca environment data.</param>
        /// <param name="securityKey">Alpaca security key.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        public static IServiceCollection AddAlpacaDataClient(
            this IServiceCollection services,
            IEnvironment environment,
            SecurityKey securityKey) =>
            services
                .AddHttpClient<IAlpacaDataClient>()
                .AddTypedClient<IAlpacaDataClient>(
                    httpClient => new AlpacaDataClient(
                        environment
                            .GetAlpacaDataClientConfiguration(securityKey)
                            .withFactoryCreatedHttpClient(httpClient)))
                .Services;

        /// <summary>
        /// Registers the concrete implementation of the <see cref="IAlpacaTradingClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="environment">Alpaca environment data.</param>
        /// <param name="securityKey">Alpaca security key.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        public static IServiceCollection AddAlpacaTradingClient(
            this IServiceCollection services,
            IEnvironment environment,
            SecurityKey securityKey) =>
            services
                .AddHttpClient<IAlpacaTradingClient>()
                .AddTypedClient<IAlpacaTradingClient>(
                    httpClient => new AlpacaTradingClient(
                        environment
                            .GetAlpacaTradingClientConfiguration(securityKey)
                            .withFactoryCreatedHttpClient(httpClient)))
                .Services;

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
