using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Alpaca.Markets.Extensions.Tests;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class MockClientsFactoryFixture
{
    private readonly SecurityKey _securityKey = new SecretKey(
        Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

    public MockClient<AlpacaDataClientConfiguration, IAlpacaDataClient> GetAlpacaDataClientMock(
        AlpacaDataClientConfiguration? configuration = null) =>
        new (configuration ?? Environments.Live.GetAlpacaDataClientConfiguration(_securityKey), _ => _.GetClient());

    public MockClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> GetAlpacaTradingClientMock(
        AlpacaTradingClientConfiguration? configuration = null) =>
        new (configuration ?? Environments.Live.GetAlpacaTradingClientConfiguration(_securityKey), _ => _.GetClient());

    public MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> GetAlpacaCryptoDataClientMock(
        AlpacaCryptoDataClientConfiguration? configuration = null) =>
        new (configuration ?? Environments.Live.GetAlpacaCryptoDataClientConfiguration(_securityKey), _ => _.GetClient());
}

[CollectionDefinition("MockEnvironment")]
public sealed class MockClientsFactoryCollection
    : ICollectionFixture<MockClientsFactoryFixture>
{
}
