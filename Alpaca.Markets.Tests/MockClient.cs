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

    public void AddGet<TJson>(
        String request,
        TJson response) =>
        Handler.Expect(request).RespondJson(response);

    public void AddGet<TJson>(
        String request,
        TJson[] responses) =>
        Handler.Expect(request).RespondJson(responses);

    public void AddPatch<TJson>(
        String request,
        TJson response) =>
        Handler.Expect(HttpMethod.Patch, request).RespondJson(response);

    public void Dispose()
    {
        Handler.VerifyNoOutstandingExpectation();
        Handler.Dispose();

        Client.Dispose();
    }
}
