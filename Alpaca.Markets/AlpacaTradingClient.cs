namespace Alpaca.Markets;

internal sealed partial class AlpacaTradingClient : IAlpacaTradingClient
{
    private readonly HttpClient _httpClient;

    internal AlpacaTradingClient(
        AlpacaTradingClientConfiguration configuration) =>
        _httpClient = configuration.EnsureNotNull().GetConfiguredHttpClient();

    public void Dispose() => _httpClient.Dispose();
}
