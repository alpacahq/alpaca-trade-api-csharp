using RichardSzalay.MockHttp;

namespace Alpaca.Markets.Extensions.Tests;

public sealed class MockClient<TClientConfiguration, TClient> : IDisposable
    where TClientConfiguration : AlpacaClientConfigurationBase
    where TClient : class, IDisposable
{
    private readonly MockHttpMessageHandler _handler = new();

    public MockClient(
        TClientConfiguration configuration,
        Func<TClientConfiguration, TClient> factory)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        configuration.HttpClient = _handler.ToHttpClient();
        Client = factory(configuration);
    }

    public TClient Client { get; }

    public void AddGet(
        String request,
        JToken response) =>
        addExpectRespond(HttpMethod.Get, request, response);

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
