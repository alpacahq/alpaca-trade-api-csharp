using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Alpaca.Markets.Extensions
{
    public static class PolygonServiceCollectionExtensions
    {
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