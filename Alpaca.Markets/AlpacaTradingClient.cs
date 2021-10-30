using System.Net.Http;

namespace Alpaca.Markets
{
    internal sealed partial class AlpacaTradingClient : IAlpacaTradingClient
    {
        private readonly HttpClient _httpClient;

        public AlpacaTradingClient(
            AlpacaTradingClientConfiguration configuration) =>
            _httpClient = configuration
                .EnsureNotNull(nameof(configuration))
                .GetConfiguredHttpClient();

        public void Dispose() => _httpClient.Dispose();
    }
}
