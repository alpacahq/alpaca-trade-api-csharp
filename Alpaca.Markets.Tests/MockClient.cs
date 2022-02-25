namespace Alpaca.Markets.Tests;

public sealed class MockClient<TConfiguration, TClient> : IMock, IDisposable
    where TConfiguration : AlpacaClientConfigurationBase
    where TClient : class, IDisposable
{
    private readonly MockHttpMessageHandler _handler = new ();

    public MockClient(
        TConfiguration configuration,
        Func<TConfiguration, TClient> factory)
    {
        configuration.HttpClient = _handler.ToHttpClient();
        Client = factory(configuration);
    }

    public TClient Client { get; }

    public void AddGet(
        String request,
        JToken response) =>
        addExpectRespond(HttpMethod.Get, request, response);

    public void AddGet(
        String request,
        String response,
        HttpStatusCode code) =>
        _handler.Expect(HttpMethod.Get, request).Respond(code, new StringContent(response));

    public void AddPut(
        String request,
        JToken response) =>
        addExpectRespond(HttpMethod.Put, request, response);

    public void AddPost(
        String request,
        JToken response) =>
        addExpectRespond(HttpMethod.Post, request, response);

    public void AddPatch(
        String request,
        JToken response) =>
        addExpectRespond(HttpMethod.Patch, request, response);

    public void AddDelete(
        String request,
        JToken response) =>
        addExpectRespond(HttpMethod.Delete, request, response);

    public void AddDelete(
        String request,
        String response,
        HttpStatusCode code) =>
        _handler.Expect(HttpMethod.Delete, request).Respond(code, new StringContent(response));

    public void Dispose()
    {
        _handler.VerifyNoOutstandingExpectation();
        _handler.Dispose();

        Client.Dispose();
    }

    private void addExpectRespond(
        HttpMethod method,
        String request,
        JToken response) =>
        _handler.Expect(method, request)
            .Respond("application/json", response.ToString());
}
