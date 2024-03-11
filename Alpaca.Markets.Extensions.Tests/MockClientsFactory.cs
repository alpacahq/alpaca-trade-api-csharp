using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Extensions.Tests;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class MockClientsFactoryFixture
{
    private readonly SecurityKey _secretKey = new SecretKey(
        Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

    private readonly SecurityKey _oauthKey = new OAuthKey(Guid.NewGuid().ToString("N"));

    public MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> GetAlpacaDataClientMock(
        AlpacaDataClientConfiguration? configuration = null) =>
        new(configuration ?? Environments.Paper.GetAlpacaDataClientConfiguration(_secretKey), clientConfiguration => clientConfiguration.GetClient());

    public MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> GetAlpacaTradingClientMock(
        AlpacaTradingClientConfiguration? configuration = null) =>
        new(configuration ?? Environments.Paper.GetAlpacaTradingClientConfiguration(_oauthKey), clientConfiguration => clientConfiguration.GetClient());

    public MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> GetAlpacaCryptoDataClientMock(
        AlpacaCryptoDataClientConfiguration? configuration = null) =>
        new(configuration ?? Environments.Paper.GetAlpacaCryptoDataClientConfiguration(_secretKey), clientConfiguration => clientConfiguration.GetClient());

    public MockClient<AlpacaOptionsDataClientConfiguration, IAlpacaOptionsDataClient> GetAlpacaOptionsDataClientMock(
        AlpacaOptionsDataClientConfiguration? configuration = null) =>
        new(configuration ?? Environments.Paper.GetAlpacaOptionsDataClientConfiguration(_secretKey), clientConfiguration => clientConfiguration.GetClient());
}

[CollectionDefinition("MockEnvironment")]
public sealed class MockClientsFactoryCollection
    : ICollectionFixture<MockClientsFactoryFixture>;
