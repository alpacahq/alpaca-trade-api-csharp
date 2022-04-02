using System.Diagnostics.CodeAnalysis;
using Alpaca.Markets.Extensions;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class MockClientsFactoryFixture
{
    private readonly SecurityKey _secretKey = new SecretKey(
        Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

    private readonly SecurityKey _oauthKey = new OAuthKey(Guid.NewGuid().ToString("N"));

    public MockWsClient<AlpacaStreamingClientConfiguration, IAlpacaStreamingClient> GetAlpacaStreamingClientMock(
        AlpacaStreamingClientConfiguration? configuration = null) =>
        new (configuration ?? Environments.Live.GetAlpacaStreamingClientConfiguration(_secretKey), _ => _.GetClient());

    public MockHttpClient<AlpacaDataClientConfiguration, IAlpacaDataClient> GetAlpacaDataClientMock(
        AlpacaDataClientConfiguration? configuration = null) =>
        new (configuration ?? Environments.Live.GetAlpacaDataClientConfiguration(_secretKey), _ => _.GetClient());

    public MockHttpClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> GetAlpacaTradingClientMock(
        AlpacaTradingClientConfiguration? configuration = null) =>
        new (configuration ?? Environments.Live.GetAlpacaTradingClientConfiguration(_oauthKey), _ => _.GetClient());

    public MockHttpClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> GetAlpacaCryptoDataClientMock(
        AlpacaCryptoDataClientConfiguration? configuration = null) =>
        new (configuration ?? Environments.Live.GetAlpacaCryptoDataClientConfiguration(_secretKey),_ => _.GetClient());
}

[CollectionDefinition("MockEnvironment")]
public sealed class MockClientsFactoryCollection
    : ICollectionFixture<MockClientsFactoryFixture>
{
}
