using System.Net.Http;
using System.Net.Http.Headers;

namespace Alpaca.Markets
{
    internal sealed partial class AlpacaTradingClient : IAlpacaTradingClient
    {
        private readonly HttpClient _httpClient;

        public AlpacaTradingClient(
            AlpacaTradingClientConfiguration configuration)
        {
            configuration
                .EnsureNotNull(nameof(configuration))
                .EnsureIsValid();

            _httpClient = configuration.HttpClient ??
                configuration.ThrottleParameters.GetHttpClient();

            _httpClient.AddAuthenticationHeaders(configuration.SecurityId);

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding
                .Add(new StringWithQualityHeaderValue("gzip"));
            _httpClient.BaseAddress = configuration.ApiEndpoint;
            _httpClient.SetSecurityProtocol();
        }

        public void Dispose() => _httpClient.Dispose();
    }
}
