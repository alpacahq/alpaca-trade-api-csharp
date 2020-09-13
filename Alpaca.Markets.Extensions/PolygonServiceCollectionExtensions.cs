using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extensions methods for registering the strongly-typed Polygon.io REST API clients
    /// in the default Microsoft dependency injection container used by the most .NET hosts.
    /// </summary>
    public static class PolygonServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the concrete implementation of the <see cref="IPolygonDataClient"/>
        /// interface in the services catalog and make it available in constructors.
        /// </summary>
        /// <param name="services">Registered services collection.</param>
        /// <param name="environment">Alpaca environment data.</param>
        /// <param name="keyId">Alpaca API key used for Polygon.io connection.</param>
        /// <returns>The <paramref name="services"/> object (fluent interface).</returns>
        public static IServiceCollection AddPolygonDataClient(
            this IServiceCollection services,
            IEnvironment environment,
            String keyId) =>
            services
                .AddHttpClient<IPolygonDataClient>()
                .AddTypedClient<IPolygonDataClient>(
                    httpClient => new PolygonDataClient(
                        environment
                            .GetPolygonDataClientConfiguration(keyId)
                            .withFactoryCreatedHttpClient(httpClient)))
                .Services;

        private static PolygonDataClientConfiguration withFactoryCreatedHttpClient(
            this PolygonDataClientConfiguration configuration,
            HttpClient httpClient)
        {
            configuration.HttpClient = httpClient;
            return configuration;
        }
    }
}