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
        addExpectRespond(HttpMethod.Get, request, response);

    public void AddPatch<TJson>(
        String request,
        TJson response) =>
        addExpectRespond(HttpMethod.Patch, request, response);

    public void AddPost<TJson>(
        String request,
        TJson response) =>
        addExpectRespond(HttpMethod.Post, request, response);

    public void AddDelete<TJson>(
        String request,
        TJson response) =>
        addExpectRespond(HttpMethod.Delete, request, response);

    public void Dispose()
    {
        Handler.VerifyNoOutstandingExpectation();
        Handler.Dispose();

        Client.Dispose();
    }

    private void addExpectRespond<TJson>(
        HttpMethod method,
        String request,
        TJson response) =>
        Handler.Expect(method, request).RespondJson(response);
}
