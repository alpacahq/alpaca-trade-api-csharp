using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Alpaca.Markets.Extensions
{
    public static class AlpacaServiceCollectionExtensions
    {
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
