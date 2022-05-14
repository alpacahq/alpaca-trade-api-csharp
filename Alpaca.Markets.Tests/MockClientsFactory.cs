using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Alpaca.Markets.Extensions;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public sealed class MockClientsFactoryFixture
{
    private readonly SecurityKey _secretKey = new SecretKey(
        Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

    private readonly SecurityKey _oauthKey = new OAuthKey(Guid.NewGuid().ToString("N"));

    public MockHttpClient<AlpacaDataClientConfiguration, IAlpacaDataClient> GetAlpacaDataClientMock(
        IEnvironment? environment = null,
        AlpacaDataClientConfiguration? configuration = null) =>
        new (configuration ?? getEnvironment(environment).GetAlpacaDataClientConfiguration(_secretKey), _ => _.GetClient());

    public MockHttpClient<AlpacaTradingClientConfiguration, IAlpacaTradingClient> GetAlpacaTradingClientMock(
        IEnvironment? environment = null,
        AlpacaTradingClientConfiguration? configuration = null) =>
        new (configuration ?? getEnvironment(environment).GetAlpacaTradingClientConfiguration(_oauthKey), _ => _.GetClient());

    public MockHttpClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> GetAlpacaCryptoDataClientMock(
        IEnvironment? environment = null,
        AlpacaCryptoDataClientConfiguration? configuration = null) =>
        new (configuration ?? getEnvironment(environment).GetAlpacaCryptoDataClientConfiguration(_secretKey),_ => _.GetClient());

    public MockWsClient<AlpacaStreamingClientConfiguration, IAlpacaStreamingClient> GetAlpacaStreamingClientMock(
        IEnvironment? environment = null,
        AlpacaStreamingClientConfiguration? configuration = null) =>
        new (configuration ?? getEnvironment(environment).GetAlpacaStreamingClientConfiguration(_oauthKey), _ => _.GetClient());

    public MockWsClient<AlpacaNewsStreamingClientConfiguration, IAlpacaNewsStreamingClient> GetAlpacaNewsStreamingClientMock(
        IEnvironment? environment = null,
        AlpacaNewsStreamingClientConfiguration? configuration = null) =>
        new (configuration ?? getEnvironment(environment).GetAlpacaNewsStreamingClientConfiguration(_secretKey), _ => _.GetClient());

    public MockWsClient<AlpacaDataStreamingClientConfiguration, IAlpacaDataStreamingClient> GetAlpacaDataStreamingClientMock(
        IEnvironment? environment = null,
        AlpacaDataStreamingClientConfiguration? configuration = null) =>
        new (configuration ?? getEnvironment(environment).GetAlpacaDataStreamingClientConfiguration(_secretKey), _ => _.GetClient());

    public MockWsClient<AlpacaCryptoStreamingClientConfiguration, IAlpacaCryptoStreamingClient> GetAlpacaCryptoStreamingClientMock(
        IEnvironment? environment = null,
        AlpacaCryptoStreamingClientConfiguration? configuration = null) =>
        new (configuration ?? getEnvironment(environment).GetAlpacaCryptoStreamingClientConfiguration(_secretKey), _ => _.GetClient());

    private static IEnvironment getEnvironment(
        IEnvironment? environment) =>
        environment ?? Environments.Live;
}

public sealed class EnvironmentTestData : IEnumerable<Object[]>
{
    public IEnumerator<Object[]> GetEnumerator()
    {
        yield return new Object[] { Environments.Paper };
        yield return new Object[] { Environments.Live };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

[CollectionDefinition("MockEnvironment")]
public sealed class MockClientsFactoryCollection
    : ICollectionFixture<MockClientsFactoryFixture>
{
}
