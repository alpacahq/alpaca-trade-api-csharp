using Microsoft.Extensions.Http;

namespace Alpaca.Markets.Tests;

internal sealed class MockHttpClient<TConfiguration, TClient> : IMock, IDisposable
    where TConfiguration : AlpacaClientConfigurationBase
    where TClient : class, IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();

    public MockHttpClient(
        TConfiguration configuration,
        Func<TConfiguration, TClient> factory)
    {
        configuration.HttpClient = getHttpClient(configuration.ThrottleParameters);
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
        HttpStatusCode code,
        params KeyValuePair<String, String>[] headers) =>
        _handler.Expect(HttpMethod.Get, request).Respond(code, headers, new StringContent(response));

    public void AddGet(
        String request,
        Exception exception) =>
        _handler.Expect(HttpMethod.Get, request).Throw(exception);

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

    private HttpClient getHttpClient(
        ThrottleParameters? throttleParameters) =>
        throttleParameters is not null
            ? new HttpClient(new PolicyHttpMessageHandler(
                throttleParameters.GetAsyncPolicy())
            {
                InnerHandler = _handler
            })
            : _handler.ToHttpClient();

    private void addExpectRespond(
        HttpMethod method,
        String request,
        JToken response) =>
        _handler.Expect(method, request)
            .Respond("application/json", response.ToString());
}
