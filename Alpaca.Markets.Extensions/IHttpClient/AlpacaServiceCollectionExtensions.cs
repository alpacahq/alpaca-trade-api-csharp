using Microsoft.Extensions.DependencyInjection;

namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extensions methods for registering the strongly-typed Alpaca REST API clients
/// in the default Microsoft dependency injection container used by most .NET hosts.
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
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/>, <paramref name="environment"/>, or <paramref name="securityKey"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaCryptoDataClient(
        this IServiceCollection services,
        IEnvironment environment,
        SecurityKey securityKey) =>
        services.EnsureNotNull().AddAlpacaCryptoDataClient(environment.EnsureNotNull()
            .GetAlpacaCryptoDataClientConfiguration(securityKey.EnsureNotNull()));

    /// <summary>
    /// Registers the concrete implementation of the <see cref="IAlpacaCryptoDataClient"/>
    /// interface in the services catalog and make it available in constructors.
    /// </summary>
    /// <param name="services">Registered services collection.</param>
    /// <param name="configuration">Alpaca data client configuration.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/> or <paramref name="configuration"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaCryptoDataClient(
        this IServiceCollection services,
        AlpacaCryptoDataClientConfiguration configuration) =>
        services.EnsureNotNull()
            .AddHttpClient<IAlpacaCryptoDataClient>()
            .AddTypedClient(httpClient => configuration.EnsureNotNull()
                .withFactoryCreatedHttpClient(httpClient).GetClient())
            .withConfiguredPrimaryHttpMessageHandler(configuration.EnsureNotNull());

    /// <summary>
    /// Registers the concrete implementation of the <see cref="IAlpacaDataClient"/>
    /// interface in the services catalog and make it available in constructors.
    /// </summary>
    /// <param name="services">Registered services collection.</param>
    /// <param name="environment">Alpaca environment data.</param>
    /// <param name="securityKey">Alpaca security key.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/>, <paramref name="environment"/>, or <paramref name="securityKey"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaDataClient(
        this IServiceCollection services,
        IEnvironment environment,
        SecurityKey securityKey) =>
        services.EnsureNotNull().AddAlpacaDataClient(environment.EnsureNotNull()
            .GetAlpacaDataClientConfiguration(securityKey.EnsureNotNull()));

    /// <summary>
    /// Registers the concrete implementation of the <see cref="IAlpacaDataClient"/>
    /// interface in the services catalog and make it available in constructors.
    /// </summary>
    /// <param name="services">Registered services collection.</param>
    /// <param name="configuration">Alpaca data client configuration.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/> or <paramref name="configuration"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaDataClient(
        this IServiceCollection services,
        AlpacaDataClientConfiguration configuration) =>
        services.EnsureNotNull()
            .AddHttpClient<IAlpacaDataClient>()
            .AddTypedClient(httpClient => configuration.EnsureNotNull()
                .withFactoryCreatedHttpClient(httpClient).GetClient())
            .withConfiguredPrimaryHttpMessageHandler(configuration.EnsureNotNull());

    /// <summary>
    /// Registers the concrete implementation of the <see cref="IAlpacaTradingClient"/>
    /// interface in the services catalog and make it available in constructors.
    /// </summary>
    /// <param name="services">Registered services collection.</param>
    /// <param name="environment">Alpaca environment data.</param>
    /// <param name="securityKey">Alpaca security key.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/>, <paramref name="environment"/>, or <paramref name="securityKey"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaTradingClient(
        this IServiceCollection services,
        IEnvironment environment,
        SecurityKey securityKey) =>
        services.EnsureNotNull().AddAlpacaTradingClient(environment.EnsureNotNull()
            .GetAlpacaTradingClientConfiguration(securityKey.EnsureNotNull()));

    /// <summary>
    /// Registers the concrete implementation of the <see cref="IAlpacaTradingClient"/>
    /// interface in the services catalog and make it available in constructors.
    /// </summary>
    /// <param name="services">Registered services collection.</param>
    /// <param name="configuration">Alpaca trading client configuration.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/> or <paramref name="configuration"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaTradingClient(
        this IServiceCollection services,
        AlpacaTradingClientConfiguration configuration) =>
        services.EnsureNotNull()
            .AddHttpClient<IAlpacaTradingClient>()
            .AddTypedClient(httpClient => configuration.EnsureNotNull()
                .withFactoryCreatedHttpClient(httpClient).GetClient())
            .withConfiguredPrimaryHttpMessageHandler(configuration.EnsureNotNull());

    /// <summary>
    /// Registers the concrete implementation of the <see cref="IAlpacaOptionsDataClient"/>
    /// interface in the services catalog and make it available in constructors.
    /// </summary>
    /// <param name="services">Registered services collection.</param>
    /// <param name="environment">Alpaca environment data.</param>
    /// <param name="securityKey">Alpaca security key.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/>, <paramref name="environment"/>, or <paramref name="securityKey"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaOptionsDataClient(
        this IServiceCollection services,
        IEnvironment environment,
        SecurityKey securityKey) =>
        services.EnsureNotNull().AddAlpacaOptionsDataClient(environment.EnsureNotNull()
            .GetAlpacaOptionsDataClientConfiguration(securityKey.EnsureNotNull()));

    /// <summary>
    /// Registers the concrete implementation of the <see cref="IAlpacaOptionsDataClient"/>
    /// interface in the services catalog and make it available in constructors.
    /// </summary>
    /// <param name="services">Registered services collection.</param>
    /// <param name="configuration">Alpaca data client configuration.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="services"/> or <paramref name="configuration"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>The <see cref="IHttpClientBuilder"/> interface for further HTTP client customization.</returns>
    [UsedImplicitly]
    public static IHttpClientBuilder AddAlpacaOptionsDataClient(
        this IServiceCollection services,
        AlpacaOptionsDataClientConfiguration configuration) =>
        services.EnsureNotNull()
            .AddHttpClient<IAlpacaOptionsDataClient>()
            .AddTypedClient(httpClient => configuration.EnsureNotNull()
                .withFactoryCreatedHttpClient(httpClient).GetClient())
            .withConfiguredPrimaryHttpMessageHandler(configuration.EnsureNotNull());

    private static TConfiguration withFactoryCreatedHttpClient<TConfiguration>(
        this TConfiguration configuration,
        HttpClient httpClient)
        where TConfiguration : AlpacaClientConfigurationBase
    {
        configuration.HttpClient = httpClient;
        return configuration;
    }

    private static IHttpClientBuilder withConfiguredPrimaryHttpMessageHandler<TConfiguration>(
        this IHttpClientBuilder builder,
        TConfiguration configuration)
        where TConfiguration : AlpacaClientConfigurationBase =>
        builder.ConfigurePrimaryHttpMessageHandler(
            () => configuration.ThrottleParameters.GetMessageHandler());
}
