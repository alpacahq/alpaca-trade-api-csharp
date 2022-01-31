using RichardSzalay.MockHttp;

namespace Alpaca.Markets.Tests;

public sealed class MockClient<TClientConfiguration, TClient> : IDisposable
    where TClientConfiguration : AlpacaClientConfigurationBase
    where TClient : class, IDisposable
{
    public MockClient(
        TClientConfiguration configuration,
        Func<TClientConfiguration, TClient> factory)
    {
        configuration.HttpClient = Handler.ToHttpClient();
        Client = factory(configuration);
    }

    public MockHttpMessageHandler Handler { get; } = new ();

    public TClient Client { get; }

    public void Dispose()
    {
        Client.Dispose();
        Handler.Dispose();
    }
}