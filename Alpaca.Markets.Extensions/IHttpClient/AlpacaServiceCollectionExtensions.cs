using Microsoft.Extensions.DependencyInjection;

namespace Alpaca.Markets.Extensions;

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
            .AddTypedClient(httpClient => configuration.withFactoryCreatedHttpClient(httpClient).GetClient())
            .withConfiguredPrimaryHttpMessageHandler(configuration);

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
            .AddTypedClient(httpClient => configuration.withFactoryCreatedHttpClient(httpClient).GetClient())
            .withConfiguredPrimaryHttpMessageHandler(configuration);

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
            .AddTypedClient(httpClient => configuration.withFactoryCreatedHttpClient(httpClient).GetClient())
            .withConfiguredPrimaryHttpMessageHandler(configuration);

    private static TConfiguration withFactoryCreatedHttpClient<TConfiguration>(
        this TConfiguration configuration,
        HttpClient httpClient)
        where TConfiguration : AlpacaClientConfigurationBase
    {
        configuration.HttpClient = httpClient;
        return configuration;
    }

    private static IServiceCollection withConfiguredPrimaryHttpMessageHandler<TConfiguration>(
        this IHttpClientBuilder builder,
        TConfiguration configuration)
        where TConfiguration : AlpacaClientConfigurationBase =>
        builder
            .ConfigurePrimaryHttpMessageHandler(() => configuration.ThrottleParameters.GetMessageHandler())
            .Services;
}
