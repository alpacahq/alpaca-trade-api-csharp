namespace Alpaca.Markets;

internal sealed partial class AlpacaTradingClient : IAlpacaTradingClient
{
    private readonly RateLimitHandler _rateLimitHandler = new();

    private readonly HttpClient _httpClient;

    internal AlpacaTradingClient(
        AlpacaTradingClientConfiguration configuration) =>
        _httpClient = configuration.EnsureNotNull().GetConfiguredHttpClient();

    public IRateLimitValues GetRateLimitValues() => _rateLimitHandler.GetCurrent();

    public void Dispose()
    {
        _httpClient.Dispose();
        _rateLimitHandler.Dispose();
    }
}
