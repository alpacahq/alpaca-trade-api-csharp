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
        new(configuration ?? Environments.Paper.GetAlpacaDataClientConfiguration(_secretKey), _ => _.GetClient());

    public MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> GetAlpacaTradingClientMock(
        AlpacaTradingClientConfiguration? configuration = null) =>
        new(configuration ?? Environments.Paper.GetAlpacaTradingClientConfiguration(_oauthKey), _ => _.GetClient());

    public MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> GetAlpacaCryptoDataClientMock(
        AlpacaCryptoDataClientConfiguration? configuration = null) =>
        new(configuration ?? Environments.Paper.GetAlpacaCryptoDataClientConfiguration(_secretKey), _ => _.GetClient());
}

[CollectionDefinition("MockEnvironment")]
public sealed class MockClientsFactoryCollection
    : ICollectionFixture<MockClientsFactoryFixture>
{
}
