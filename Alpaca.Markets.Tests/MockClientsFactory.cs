using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class MockClientsFactoryFixture
{
    private readonly SecurityKey _securityKey = new SecretKey(
        Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

    public MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> GetAlpacaDataClientMock() =>
        new (Environments.Live.GetAlpacaDataClientConfiguration(_securityKey), _ => new AlpacaDataClient(_));

    public MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> GetAlpacaTradingClientMock() =>
        new (Environments.Paper.GetAlpacaTradingClientConfiguration(_securityKey), _ => new AlpacaTradingClient(_));

    public MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> GetAlpacaCryptoDataClientMock() =>
        new (Environments.Live.GetAlpacaCryptoDataClientConfiguration(_securityKey), _ => new AlpacaCryptoDataClient(_));
}

[CollectionDefinition("MockEnvironment")]
public sealed class MockClientsFactoryCollection
    : ICollectionFixture<MockClientsFactoryFixture>
{
}
